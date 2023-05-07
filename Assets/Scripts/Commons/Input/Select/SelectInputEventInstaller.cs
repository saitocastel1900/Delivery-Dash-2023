using System;
using Zenject;

namespace Commons.Input
{
    public class SelectInputEventInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(ISelectInputEventProvider), typeof(IInitializable),typeof(IDisposable)).To<SelectKeyInputProvider>()
                .AsSingle();
        }
    }
}