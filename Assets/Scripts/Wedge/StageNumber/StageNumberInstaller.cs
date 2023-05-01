using Zenject;

namespace Wedge.InGame.StageNumber
{
    public class StageNumberInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(StageNumberPresenter), typeof(IInitializable)).To<StageNumberPresenter>().AsCached()
                .NonLazy();
            Container.Bind<IStageNumberModel>().To<StageNumberModel>().FromNew().AsCached();
        }
    }
}