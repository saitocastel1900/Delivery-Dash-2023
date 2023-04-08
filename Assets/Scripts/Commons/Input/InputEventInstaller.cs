using Commons.Input;
using Zenject;

public class InputEventInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
#if UNITY_EDITOR
        Container.Bind<IInputEventProvider>()
            .To<MouseInputProvider>().AsCached();
#elif UNITY_ANDROID
        Container.Bind<IInputEventProvider>()
            .To<TouchInputProvider>().AsCached();
#endif
    }
}