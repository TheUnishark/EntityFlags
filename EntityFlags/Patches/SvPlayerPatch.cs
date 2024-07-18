using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using BrokeProtocol.GameSource;
using EntityFlags.Events;
using HarmonyLib;
using UnityEngine;

namespace EntityFlags.Patches
{
    [HarmonyPatch(typeof(SvPlayer), nameof(SvPlayer.SvView))]
    class SvView
    {
        // Flag: nosteal
        static bool Prefix(int otherID, bool force = false) => !EntityCollections.FindByID<ShEntity>(otherID).HasFlag("nosteal");
    }

    [HarmonyPatch(typeof(SvPlayer), nameof(SvPlayer.LookTactical))]
    class LookTactical
    {
        // Flag: norotate
        static bool Prefix(SvPlayer __instance, Vector3 fallback)
        {
            bool isGoTo = __instance.currentState.index == BrokeProtocol.GameSource.Core.GoTo.index;
            bool isOnDestination = __instance.player.GamePlayer().OnDestination();
            bool canRotate = !Players.noRotate.Contains(__instance.player) || !isGoTo || (isGoTo && !isOnDestination);

            if (!canRotate)
                __instance.LookAt(fallback);

            return canRotate;
        }
    }
}
