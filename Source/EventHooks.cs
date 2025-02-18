using System.Threading.Tasks;
using BeatSaberAP;
using BS_Utils.Utilities;
using SongDetailsCache;
using SongDetailsCache.Structs;

public static class EventHooks {
    private static SongDetails songdetails = null;
    private static Task<SongDetails> sdtask;

    public static void SetupHooks() {
        sdtask = SongDetails.Init();
        BSEvents.levelFailed += OnLevelFailed;
        BSEvents.levelCleared += OnLevelCleared;
    }
    private static void OnLevelFailed(StandardLevelScenesTransitionSetupDataSO setupdata, LevelCompletionResults results) {
        string cause = $"Failed to clear {setupdata.beatmapLevel.songName}";
        APConnection.SendDeathLink(cause);
    }
    private static async void OnLevelCleared(StandardLevelScenesTransitionSetupDataSO setupdata, LevelCompletionResults results) {
        if (setupdata.beatmapKey.levelId.StartsWith(CustomLevelLoader.kCustomLevelPrefixId)) {
            string songhash = setupdata.beatmapKey.levelId[CustomLevelLoader.kCustomLevelPrefixId.Length..];
            songdetails ??= await sdtask;
            songdetails.songs.FindByHash(songhash, out Song s);
            Plugin.Log.Info("BeatSaver MapID: " + s.mapId);
        }
    }
}
