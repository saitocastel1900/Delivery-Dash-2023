using Zenject;

namespace Widget.InGame.StageNumber
{
    public class StageNumberInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(StageNumberPresenter), typeof(IInitializable)).To<StageNumberPresenter>().AsCached()
                .NonLazy();
            Container.Bind(typeof(IStageNumberModel),typeof(IInitializable)).To<StageNumberModel>().FromNew().AsCached();
        }
    }
}