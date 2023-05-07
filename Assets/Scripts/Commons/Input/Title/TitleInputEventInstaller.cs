using System;
using Zenject;

namespace Commons.Input
{
    public class TitleInputEventInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(ITitleInputEventProvider), typeof(IInitializable),typeof(IDisposable)).To<TitleKeyInputProvider>()
                .AsSingle();
        }
    }
}