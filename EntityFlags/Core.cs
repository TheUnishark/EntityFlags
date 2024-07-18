using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.AI;
using HarmonyLib;
using System.Collections.Generic;

namespace EntityFlags
{
    public class Core : Plugin
    {
        internal static bool Started = false;
        internal static Dictionary<ShPlayer, ExtendedPlayer> extendedPlayers = new();

        public static State Animation = new AnimationState();
        public static State SitRandom = new SitRandomState();
        public static State Sit = new SitState();

        public Core()
        {
            Info = new PluginInfo("EntityFlags", "ef", "Special flags for entities");

            StatesAdditive = new List<State>
            {
                Animation,
                SitRandom,
                Sit,
            };

            Harmony harmony = new("com.unishark.entityflags");
            harmony.PatchAll();
        }
    }
}
