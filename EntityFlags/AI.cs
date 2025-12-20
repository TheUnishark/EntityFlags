using BrokeProtocol.Entities;
using BrokeProtocol.GameSource;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.AI;

namespace EntityFlags
{
    public class AnimationState : State
    {
        public override bool IsBusy => true;

        public override bool EnterTest() => player.animator != null && player.ExtendedPlayer().animation != null;

        public override void EnterState()
        {
            base.EnterState();

            player.svPlayer.SvAnimatorBool(player.ExtendedPlayer().animation, true);
        }

        public override void ExitState(State nextState)
        {
            player.svPlayer.SvAnimatorBool(player.ExtendedPlayer().animation, false);

            base.ExitState(nextState);
        }
    }

    public class SitRandomState : MovingState
    {
        private ShMountable mountable;

        public override byte StateMoveMode => MoveMode.Positive;

        private bool OnMountable => player.curMount == mountable;

        public override bool IsBusy => OnMountable;

        public override bool EnterTest() => player.FindClosestMountable() != null;

        public override void EnterState()
        {
            base.EnterState();

            mountable = player.FindClosestMountable();

            if (!OnMountable)
            {
                player.svPlayer.SvDismount();
                player.svPlayer.GetPath(mountable.Position);
            }
        }

        public override bool UpdateState()
        {
            if (!base.UpdateState()) return false;

            bool arrived = player.GetControlled().Distance(mountable.Position) < Util.useDistance;

            if (arrived)
            {
                if (!OnMountable && player.CanMount(mountable, true, true, out byte seatIndex))
                {
                    player.svPlayer.SvMount(mountable, seatIndex);
                }
                else if (!OnMountable)
                {
                    player.svPlayer.ResetAI();
                }
            }
            else if (player.svPlayer.BadPath)
            {
                player.svPlayer.ResetAI();
                return false;
            }
            else if (!player.svPlayer.MoveLookNavPath())
            {
                arrived = true;
            }

            return arrived;
        }
    }

    public class SitState : MovingState
    {
        private ShMountable mountable;
        private byte seatIndex;

        public override byte StateMoveMode => MoveMode.Positive;

        private bool OnMountable => player.curMount == mountable && player.seat == seatIndex;

        public override bool IsBusy => OnMountable;

        public override bool EnterTest() => player.ExtendedPlayer().mountable != null;

        public override void EnterState()
        {
            base.EnterState();

            ExtendedPlayer extendedPlayer = player.ExtendedPlayer();

            mountable = extendedPlayer.mountable;
            seatIndex = extendedPlayer.mountableSeat;

            if (!OnMountable)
            {
                player.svPlayer.SvDismount();
                player.svPlayer.GetPath(extendedPlayer.mountable.Position);
            }
        }

        public override bool UpdateState()
        {
            if (!base.UpdateState()) return false;

            bool arrived = player.GetControlled().Distance(mountable.Position) < Util.useDistance;

            if (arrived)
            {
                if (!OnMountable && player.CanMount(mountable, true, true, out _) && mountable.occupants[seatIndex] is null)
                {
                    player.svPlayer.SvMount(mountable, seatIndex);
                }
                else if (!OnMountable)
                {
                    player.svPlayer.ResetAI();
                }
            }
            else if (player.svPlayer.BadPath)
            {
                player.svPlayer.ResetAI();
                return false;
            }
            else if (!player.svPlayer.MoveLookNavPath())
            {
                arrived = true;
            }

            return arrived;
        }
    }
}
