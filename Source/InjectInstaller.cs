using BeatSaberAP;
using CustomCampaigns.Managers;
using CustomCampaigns.UI;
using CustomCampaigns.UI.FlowCoordinators;
using CustomCampaigns.UI.GameplaySetupUI;
using CustomCampaigns.UI.LeaderboardCore;
using CustomCampaigns.UI.ViewControllers;
using UnityEngine;
using Zenject;

internal class InjectInstaller : Installer {
    public override void InstallBindings() {
        Container.BindInterfacesAndSelfTo<UnlockHooks>().AsSingle();
    }
}
