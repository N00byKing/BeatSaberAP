using System;
using BeatSaberMarkupLanguage;
using CustomCampaigns.Campaign.Missions;
using CustomCampaigns.Managers;
using HarmonyLib;
using IPA.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[HarmonyPatch]
public class LockHooks : IInitializable, IDisposable {
    [Inject] private readonly MissionLevelDetailViewController _viewCtr;
    private static Button _playButton = null;

    public void Initialize() {
        _playButton = _viewCtr.GetField<Button, MissionLevelDetailViewController>("_playButton");
    }
    public void Dispose() {
        // Probably not needed
        _playButton = null;
    }

    [HarmonyPostfix, HarmonyPatch(typeof(CustomCampaignManager), "OnDidSelectMissionNode")]
    private static void HookMissionSelect(CustomCampaignManager __instance, MissionNodeVisualController missionNodeVisualController) {
        if (__instance.Campaign.info.name != APConnection.CampaignName) return;
        BeatmapLevel level = (missionNodeVisualController.missionNode.missionData as CustomMissionDataSO)?.mission.FindSong();
        if (level == null) return; // Download button is being shown
        string songhash = level.levelID[CustomLevelLoader.kCustomLevelPrefixId.Length..];
        DenyTrackIfNotUnlockedAsync(songhash);
    }

    [HarmonyPostfix, HarmonyPatch(typeof(CustomCampaignManager), "OnSongsLoaded")]
    private static void HookSongsLoaded(CustomCampaignManager __instance) {
        if (__instance.Campaign.info.name != APConnection.CampaignName) return;
        _playButton.interactable = false;
        _playButton.SetButtonText("Reopen campaign");
    }
    private static async void DenyTrackIfNotUnlockedAsync(string hash) {
        uint mapid = await BeatSaberAP.Plugin.GetMapIDFromHashAsync(hash);
        if (APConnection.HaveSong(mapid)) {
            _playButton.interactable = true;
            _playButton.SetButtonText("PLAY");
        } else {
            _playButton.interactable = false;
            _playButton.SetButtonText("AP: Missing Song Item");
        }
    }
}
