using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using HarmonyLib;

namespace EntityFlags.Patches
{
    [HarmonyPatch(typeof(SvPlayer), nameof(SvPlayer.SvView))]
    class SvView
    {
        // Flag: nosteal
        static bool Prefix(int otherID, bool force = false) => !EntityCollections.FindByID<ShEntity>(otherID).HasFlag("nosteal");
    }
}
