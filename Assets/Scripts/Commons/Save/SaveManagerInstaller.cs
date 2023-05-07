using Zenject;

namespace Commons.Save
{
    public class SaveManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SaveManager>().AsSingle().NonLazy();
        }
    }
}