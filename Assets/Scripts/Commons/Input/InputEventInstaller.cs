using Commons.Input;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class InputEventInstaller : MonoInstaller
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Button _aheadButton;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Button _leftButton;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Button _rightButton;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Button _backButton;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Button _undoButton;

    public override void InstallBindings()
    {
#if UNITY_EDITOR
        Container.Bind(typeof(IInputEventProvider), typeof(IInitializable)).To<KeyInputProvider>()
            .AsCached();
#elif UNITY_ANDROID
        Container.Bind(typeof(IInputEventProvider),typeof(IInitializable)).To<ButtonInputProvider>()
            .AsCached().WithArguments(_aheadButton,_leftButton,_rightButton,_backButton,_undoButton);
#endif
    }
}