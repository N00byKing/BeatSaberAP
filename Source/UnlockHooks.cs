using System;
using BeatSaberMarkupLanguage;
using CustomCampaigns.Managers;
using HarmonyLib;
using IPA.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[HarmonyPatch]
public class UnlockHooks : IInitializable, IDisposable {
    [Inject] private MissionLevelDetailViewController _viewCtr;
    private static Button _playButton = null;

    public void Initialize() {
        _playButton = _viewCtr.GetField<Button, MissionLevelDetailViewController>("_playButton");
    }
    public void Dispose() {
        // Probably not needed
        _playButton = null;
    }

    [HarmonyPostfix, HarmonyPatch(typeof(CustomCampaignManager), "OnDidSelectMissionNode")]
    private static void DenyTrackIfNotUnlocked(CustomCampaignManager __instance, MissionNodeVisualController missionNodeVisualController) {
        _playButton.interactable = false;
        _playButton.SetButtonText("AP: Missing Track Item");
    }
}
