using Zenject;

internal class InjectInstaller : Installer {
    public override void InstallBindings() {
        Container.BindInterfacesAndSelfTo<UnlockHooks>().AsSingle();
    }
}
