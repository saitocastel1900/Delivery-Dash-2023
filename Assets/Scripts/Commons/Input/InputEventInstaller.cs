using UnityEngine;
using Zenject;
using UnityEngine.UI;

namespace Commons.Input
{
    public class InputEventInstaller : MonoInstaller
    {
        /// <summary>
        /// 前方のボタン
        /// </summary>
        [SerializeField] private Button _aheadButton;

        /// <summary>
        /// 左向のボタン
        /// </summary>
        [SerializeField] private Button _leftButton;

        /// <summary>
        /// 右向のボタン
        /// </summary>
        [SerializeField] private Button _rightButton;

        /// <summary>
        /// 後方のボタン
        /// </summary>
        [SerializeField] private Button _backButton;

        /// <summary>
        /// Undoボタン
        /// </summary>
        [SerializeField] private Button _undoButton;

        public override void InstallBindings()
        {
#if UNITY_EDITOR || UNITY_WEBGL
            Container.Bind(typeof(IInputEventProvider), typeof(IInitializable)).To<KeyInputProvider>()
                .AsCached();
#elif UNITY_ANDROID
        Container.Bind(typeof(IInputEventProvider),typeof(IInitializable)).To<ButtonInputProvider>()
            .AsCached().WithArguments(_aheadButton,_leftButton,_rightButton,_backButton,_undoButton);
#endif
        }
    }
}
