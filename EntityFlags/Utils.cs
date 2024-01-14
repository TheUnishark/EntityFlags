using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using System.Linq;

namespace EntityFlags
{
    internal static class Utils
    {
        public static ExtendedPlayer ExtendedPlayer(this ShPlayer player) => Core.extendedPlayers[player];

        public static ShMountable FindClosestMountable(this ShPlayer player, bool forceSeat = false, byte seatIndex = 0)
        {
            ShMountable mountable = null;
            float distance = 0;

            foreach (ShMountable mountable1 in EntityCollections.Entities.Where(x => x is ShMountable y && y.seats.Length > 0 && y.GetPlaceIndex == player.GetPlaceIndex))
            {
                float distance1 = player.Distance(mountable1);

                if (distance1 < distance || mountable is null)
                {
                    if ((forceSeat && (seatIndex >= mountable1.seats.Length || mountable1.occupants[seatIndex] != null))
                        || !player.CanMount(mountable1, true, false, out _)) continue;

                    mountable = mountable1;
                    distance = distance1;
                }
            }

            return mountable;
        }

        public static void AnimState(this ShEntity entity)
        {
            if (entity.TryGetFlagValue("anim", out string animName))
            {
                if (entity is ShPlayer player && !Players.noAI.Contains(player))
                {
                    player.ExtendedPlayer().animation = animName;
                    player.svPlayer.SetState(Core.Animation.index);
                }
                else if (entity.animator != null)
                {
                    entity.svEntity.SvAnimatorBool(animName, true);
                }
            }
        }

        public static void MountState(this ShPlayer player)
        {
            if (!Players.noAI.Contains(player))
            {
                if (player.TryGetFlagValue("mount", out byte seatIndex))
                {
                    player.ProcessMount(seatIndex);
                    player.svPlayer.SetState(Core.Sit.index);
                }
                else if (player.HasFlag("mount"))
                {
                    player.svPlayer.SetState(Core.SitRandom.index);
                }
            }
            else
            {
                if (player.TryGetFlagValue("mount", out byte seatIndex))
                {
                    ExtendedPlayer extendedPlayer = player.ExtendedPlayer();

                    player.ProcessMount(seatIndex);
                    player.svPlayer.SvMount(extendedPlayer.mountable, extendedPlayer.mountableSeat);
                }
                else if (player.HasFlag("mount"))
                {
                    ShMountable mountable = player.FindClosestMountable();
                    player.svPlayer.SvTryMount(mountable.ID, false);
                }
            }
        }

        public static void ProcessMount(this ShPlayer player, byte seatIndex)
        {
            ExtendedPlayer extendedPlayer = player.ExtendedPlayer();

            extendedPlayer.mountable = player.FindClosestMountable(true, seatIndex);
            extendedPlayer.mountableSeat = seatIndex;
        }
    }
}
