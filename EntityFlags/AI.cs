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
        ShMountable mountable;
        private bool onMountable;

        public override byte StateMoveMode => MoveMode.Positive;
        public override bool IsBusy => onMountable;

        public override bool EnterTest() => player.FindClosestMountable() != null;

        public override void EnterState()
        {
            base.EnterState();

            mountable = player.FindClosestMountable();
            onMountable = player.curMount == mountable;

            if (!onMountable)
            {
                player.svPlayer.SvDismount();
                player.svPlayer.GetPath(mountable.GetPosition);
            }
        }

        public override bool UpdateState()
        {
            if (!base.UpdateState()) return false;

            bool arrived = player.GetControlled.Distance(mountable.GetPosition) < Util.useDistance;

            if (arrived)
            {
                if (!onMountable && player.CanMount(mountable, true, true, out byte seatIndex))
                {
                    player.svPlayer.SvMount(mountable, seatIndex);
                    onMountable = true;
                }
                else if (!onMountable)
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
        private bool onMountable;
        private ExtendedPlayer extendedPlayer;

        public override byte StateMoveMode => MoveMode.Positive;

        public override bool IsBusy => player.curMount == extendedPlayer.mountable && player.seat == extendedPlayer.mountableSeat;

        public override bool EnterTest() => player.ExtendedPlayer().mountable != null;

        public override void EnterState()
        {
            base.EnterState();

            extendedPlayer = player.ExtendedPlayer();
            onMountable = player.curMount != null && (player.seat != extendedPlayer.mountableSeat || player.curMount != extendedPlayer.mountable);

            if (!onMountable)
            {
                player.svPlayer.SvDismount();
                player.svPlayer.GetPath(extendedPlayer.mountable.GetPosition);
            }
        }

        public override bool UpdateState()
        {
            if (!base.UpdateState()) return false;

            bool arrived = player.GetControlled.Distance(extendedPlayer.mountable.GetPosition) < Util.useDistance;

            if (arrived)
            {
                if (!extendedPlayer.OnMount() && player.CanMount(extendedPlayer.mountable, true, true, out _) && extendedPlayer.mountable.occupants[extendedPlayer.mountableSeat] is null)
                {
                    player.svPlayer.SvMount(extendedPlayer.mountable, extendedPlayer.mountableSeat);
                }
                else if (!extendedPlayer.OnMount())
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
