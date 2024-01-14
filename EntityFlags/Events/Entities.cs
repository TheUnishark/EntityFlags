using BrokeProtocol.API;
using BrokeProtocol.Entities;

namespace EntityFlags.Events
{
    internal class Entities : EntityEvents
    {
        [Execution(ExecutionMode.Event)]
        public override bool Spawn(ShEntity entity)
        {
            // Flag: anim:animName
            if (Core.Started && entity.HasFlag("anim"))
                entity.AnimState();

            return true;
        }
    }
}
