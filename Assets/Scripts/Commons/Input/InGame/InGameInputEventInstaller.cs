using System;
using Zenject;

namespace Commons.Input
{
    public class InGameInputEventInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(IInGameInputEventProvider), typeof(IInitializable),typeof(IDisposable)).To<InGameKeyInputProvider>()
                .AsSingle();
        }
    }
}
