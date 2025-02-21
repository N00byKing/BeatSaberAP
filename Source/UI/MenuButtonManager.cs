using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using System;
using Zenject;

internal class MenuButtonManager : IInitializable, IDisposable {
    [Inject] private readonly MainFlowCoordinator _mainFlowCoordinator;
    [Inject] private readonly ArchipelagoFlowCoordinator _apFlowCoordinator;
    private MenuButton _menuButton;

    public void Initialize() {
        _menuButton = new MenuButton("Archipelago", PresentFlowCoordinator);
        MenuButtons.Instance.RegisterButton(_menuButton);
    }

    private void PresentFlowCoordinator() {
        _mainFlowCoordinator.PresentFlowCoordinator(_apFlowCoordinator);
    }

    public void Dispose() {
        if (MenuButtons.Instance != null && BSMLParser.Instance != null) {
            MenuButtons.Instance.UnregisterButton(_menuButton);
        }
    }
}
