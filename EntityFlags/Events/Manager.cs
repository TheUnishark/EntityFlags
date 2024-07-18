using BrokeProtocol.API;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;

namespace EntityFlags.Events
{
    internal class Manager : ManagerEvents
    {
        [Execution(ExecutionMode.Event)]
        public override bool FixedUpdate()
        {
            // Flag: noai
            foreach (ShPlayer player in Players.noAI)
            {
                if (player.svPlayer.currentState.index != BrokeProtocol.GameSource.Core.Null.index)
                    player.svPlayer.SetState(BrokeProtocol.GameSource.Core.Null.index);
            }

            return true;
        }

        [Execution(ExecutionMode.Event)]
        public override bool Start()
        {
            Core.Started = true;

            foreach (ShEntity entity in EntityCollections.Entities)
            {
                // Flag: anim:animName
                if (entity.HasFlag("anim"))
                {
                    entity.AnimState();
                }
            }

            foreach (ShPlayer player in EntityCollections.Players)
            {
                // Flag: mount:id
                if (player.HasFlag("mount"))
                {
                    player.MountState();
                }
            }

            return true;
        }
    }
}
