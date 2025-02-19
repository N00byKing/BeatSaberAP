using System.Threading.Tasks;
using System.Reflection;
using BeatSaberAP;
using HarmonyLib;
using SongDetailsCache;
using SongDetailsCache.Structs;
using CustomCampaigns.UI.FlowCoordinators;

[HarmonyPatch]
public static class EventHooks {
    private static SongDetails songdetails = null;
    private static Task<SongDetails> sdtask;

    public static void SetupHooks() {
        sdtask = SongDetails.Init();
    }

    [HarmonyPostfix, HarmonyPatch(typeof(CampaignFlowCoordinator), "HandleMissionLevelSceneDidFinish")]
    private static void OnHandleMissionLevelSceneDidFinish(CampaignFlowCoordinator __instance, MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupData, MissionCompletionResults missionCompletionResults) {
        if (CustomCampaignFlowCoordinator.CustomCampaignManager.Campaign.info.name != APConnection.CampaignName) {
            Plugin.Log.Info("Campaign is: " + CustomCampaignFlowCoordinator.CustomCampaignManager.Campaign.info.name);
            Plugin.Log.Info("Campaign ex: " + APConnection.CampaignName);
            return;
        }
        if (missionCompletionResults.levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Failed) {
            OnLevelFailed(missionLevelScenesTransitionSetupData);
        } else if (missionCompletionResults.levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared) {
            OnLevelCleared(missionLevelScenesTransitionSetupData, missionCompletionResults);
        }
    }
    private static void OnLevelFailed(MissionLevelScenesTransitionSetupDataSO setupdata) {
        GameplayCoreSceneSetupData gc_setupdata = (GameplayCoreSceneSetupData)typeof(LevelScenesTransitionSetupDataSO).GetProperty("gameplayCoreSceneSetupData", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(setupdata);
        string cause = $"Failed to clear {gc_setupdata.beatmapLevel.songName}";
        APConnection.SendDeathLink(cause);
    }
    private static async void OnLevelCleared(MissionLevelScenesTransitionSetupDataSO setupdata, MissionCompletionResults results) {
        GameplayCoreSceneSetupData gc_setupdata = (GameplayCoreSceneSetupData)typeof(LevelScenesTransitionSetupDataSO).GetProperty("gameplayCoreSceneSetupData", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(setupdata);

        songdetails ??= await sdtask;
        string songhash = gc_setupdata.beatmapKey.levelId[CustomLevelLoader.kCustomLevelPrefixId.Length..];
        songdetails.songs.FindByHash(songhash, out Song s);
        APConnection.CheckLocation(s.mapId);
    }
}
