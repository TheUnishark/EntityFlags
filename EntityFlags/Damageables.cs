using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Required;
using UnityEngine;

namespace EntityFlags
{
    internal class Damageables : DamageableEvents
    {
        // Flag: godmode
        [Execution(ExecutionMode.PreEvent)]
        public override bool Damage(ShDamageable damageable, DamageIndex damageIndex, float amount, ShPlayer attacker, Collider collider, Vector3 hitPoint, Vector3 hitNormal) => !damageable.HasFlag("godmode");
    }
}
