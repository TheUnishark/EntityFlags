using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace EntityFlags
{
    internal class Players : PlayerEvents
    {
        public static Dictionary<ShPlayer, Quaternion> noRotate = new Dictionary<ShPlayer, Quaternion>();
        public static List<ShPlayer> noAI = new List<ShPlayer>();

        [Execution(ExecutionMode.Event)]
        public override bool Initialize(ShEntity entity)
        {
            Core.extendedPlayers.Add(entity.Player, new ExtendedPlayer(entity.Player));

            // Flag: norotate
            if (entity.HasFlag("norotate")) noRotate.Add(entity.Player, entity.GetRotation);

            // Flag: noai
            if (entity.HasFlag("noai")) noAI.Add(entity.Player);

            return true;
        }

        [Execution(ExecutionMode.Event)]
        public override bool Destroy(ShEntity entity)
        {
            if (noRotate.ContainsKey(entity.Player)) noRotate.Remove(entity.Player);
            if (noAI.Contains(entity.Player)) noAI.Remove(entity.Player);

            return true;
        }

        [Execution(ExecutionMode.Event)]
        public override bool Spawn(ShEntity entity)
        {
            // Flag: mount:seatIndex
            if (Core.Started && entity.Player.HasFlag("mount"))
                entity.Player.MountState();

            return true;
        }

        // Flag: norestrain
        [Execution(ExecutionMode.PreEvent)]
        public override bool Restrain(ShPlayer player, ShPlayer initiator, ShRestrained restrained) => !player.HasFlag("norestrain");

        // Flag: norestrain
        [Execution(ExecutionMode.PreEvent)]
        public override bool RestrainOther(ShPlayer player, ShPlayer hitPlayer, ShRestraint restraint) => !hitPlayer.HasFlag("norestrain");
    }
}
