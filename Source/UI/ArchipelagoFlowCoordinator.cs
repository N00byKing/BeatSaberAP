using BeatSaberMarkupLanguage;
using HMUI;
using Zenject;

public class ArchipelagoFlowCoordinator : FlowCoordinator {

    [Inject] private readonly MainFlowCoordinator _mainFlowCoordinator;
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

    protected override void BackButtonWasPressed(ViewController topViewController) {
        _apViewCtr.connStatus.text = "";
        _mainFlowCoordinator.DismissFlowCoordinator(this);
    }
}
