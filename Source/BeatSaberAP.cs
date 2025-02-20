using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HarmonyLib;
using IPA;
using IPA.Logging;
using SiraUtil.Zenject;
using SongDetailsCache;
using SongDetailsCache.Structs;
using Zenject;

namespace BeatSaberAP
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    internal class Plugin
    {
        private Harmony harmony = null!;
        private static Task<SongDetails> sdtask;
        public static Logger Log { get; private set; }

        // setup that does not require game code
        // this is only called once ever, so do once-ever initialization
        [Init]
        public Plugin(Logger logger, Zenjector zenjector)
        {
            Log = logger;
            Log.Debug("BeatSaberAP plugin running!");

            zenjector.Install<InjectInstaller>(Location.Menu);

            APConnection.ConnectAndGetSlotData("localhost", 38281, "blok", "");
        }

        [OnStart]
        public void OnStart()
        {
            // setup that requires game code
            // Load patches from any class annotated with @HarmonyPatch
            harmony = Harmony.CreateAndPatchAll(typeof(Plugin).Assembly);

            sdtask = SongDetails.Init();
        }

        public static async Task<uint> GetMapIDFromHashAsync(string hash) {
            (await sdtask).songs.FindByHash(hash, out Song s);
            return s.mapId;
        }

        [OnExit]
        public void OnExit()
        {
            // teardown
            harmony.UnpatchSelf();
        }
    }
}
