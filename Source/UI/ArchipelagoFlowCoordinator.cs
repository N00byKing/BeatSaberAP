using BeatSaberMarkupLanguage;
using CustomCampaigns.Managers;
using CustomCampaigns.UI.ViewControllers;
using HMUI;
using UnityEngine;
using Zenject;

public class ArchipelagoFlowCoordinator : FlowCoordinator {

    [Inject] private readonly MainFlowCoordinator _mainFlowCoordinator;
    private NavigationController _navCtr;
    private ArchipelagoViewController _apViewCtr;

    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
        _apViewCtr ??= BeatSaberUI.CreateViewController<ArchipelagoViewController>();
        if (firstActivation) {
            SetTitle("Archipelago Connection Settings");
            showBackButton = true;
        }
        if (addedToHierarchy) {
            ProvideInitialViewControllers(_apViewCtr);
        }
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling) { }

    protected override void BackButtonWasPressed(ViewController topViewController) {
        _mainFlowCoordinator.DismissFlowCoordinator(this);
    }
}
