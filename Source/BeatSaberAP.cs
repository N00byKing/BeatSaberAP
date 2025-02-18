using System;
using HarmonyLib;
using IPA;
using IPA.Logging;

namespace BeatSaberAP
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    internal class Plugin
    {
        private Harmony harmony = null!;
        public static Logger Log { get; private set; }

        // setup that does not require game code
        // this is only called once ever, so do once-ever initialization
        [Init]
        public Plugin(Logger logger)
        {
            Log = logger;
            Log.Debug("BeatSaberAP plugin running!");

            APConnection.ConnectAndGetSlotData("localhost", 38281, "blok", "");
            EventHooks.SetupHooks();
        }

        [OnStart]
        public void OnStart()
        {
            // setup that requires game code
            // Load patches from any class annotated with @HarmonyPatch
            harmony = Harmony.CreateAndPatchAll(typeof(Plugin).Assembly);
        }

        [OnExit]
        public void OnExit()
        {
            // teardown
            harmony.UnpatchSelf();
        }
    }
}
