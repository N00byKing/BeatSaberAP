using BeatSaberAP;
using HarmonyLib;
using CustomCampaigns.UI.FlowCoordinators;
using IPA.Utilities;

[HarmonyPatch]
public static class EventHooks {
    [HarmonyPostfix, HarmonyPatch(typeof(CampaignFlowCoordinator), "HandleMissionLevelSceneDidFinish")]
    private static void OnHandleMissionLevelSceneDidFinish(CampaignFlowCoordinator __instance, MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupData, MissionCompletionResults missionCompletionResults) {
        APConnection.CampaignValidity validity = APConnection.CheckCampaignValid(CustomCampaignFlowCoordinator.CustomCampaignManager.Campaign.info.name);
        if (validity != APConnection.CampaignValidity.Correct) return;
        if (missionCompletionResults.levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Failed) {
            OnLevelFailed(missionLevelScenesTransitionSetupData);
        } else if (missionCompletionResults.levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared) {
            OnLevelCleared(missionLevelScenesTransitionSetupData, missionCompletionResults);
        }
    }
    private static void OnLevelFailed(MissionLevelScenesTransitionSetupDataSO setupdata) {
        GameplayCoreSceneSetupData gc_setupdata = setupdata.GetProperty<GameplayCoreSceneSetupData, LevelScenesTransitionSetupDataSO>("gameplayCoreSceneSetupData");
        string cause = $"Failed to clear {gc_setupdata.beatmapLevel.songName}";
        APConnection.SendDeathLink(cause);
    }
    private static async void OnLevelCleared(MissionLevelScenesTransitionSetupDataSO setupdata, MissionCompletionResults results) {
        GameplayCoreSceneSetupData gc_setupdata = setupdata.GetProperty<GameplayCoreSceneSetupData, LevelScenesTransitionSetupDataSO>("gameplayCoreSceneSetupData");

        string songhash = gc_setupdata.beatmapKey.levelId[CustomLevelLoader.kCustomLevelPrefixId.Length..];
        uint mapid = await Plugin.GetMapIDFromHashAsync(songhash);
        APConnection.CheckLocation(mapid);
    }
}
