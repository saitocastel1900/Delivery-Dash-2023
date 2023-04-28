using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using Zenject;

namespace Commons.Input
{
    public class ButtonInputProvider : IInputEventProvider, IInitializable
    {
        /// <summary>
        /// 前方ボタン
        /// </summary>
        private Button _aheadButton;

        /// <summary>
        /// 左向ボタン
        /// </summary>
        private Button _leftButton;

        /// <summary>
        /// 右向ボタン
        /// </summary>
        private Button _rightButton;

        /// <summary>
        /// 後方ボタン
        /// </summary>
        private Button _backButton;

        /// <summary>
        /// Undoボタン
        /// </summary>
        private Button _undoButton;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ButtonInputProvider(Button aheadButton, Button leftButton, Button rightButton, Button backButton,
            Button undoButton)
        {
            _aheadButton = aheadButton;
            _leftButton = leftButton;
            _rightButton = rightButton;
            _backButton = backButton;
            _undoButton = undoButton;
        }

        /// <summary>
        /// 進行方向
        /// </summary>
        public IReadOnlyReactiveProperty<Vector3> MoveDirection => _moveDirection;
        private ReactiveProperty<Vector3> _moveDirection = new ReactiveProperty<Vector3>();

        /// <summary>
        /// Undoボタンが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> UndoButton => _undo;
        private ReactiveProperty<bool> _undo = new ReactiveProperty<bool>();

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            //キーが押されたら、進行方向を設定する
            Observable.Merge(
                    _aheadButton.OnClickAsObservable().Select(_ => Vector3.forward),
                    _rightButton.OnClickAsObservable().Select(_ => Vector3.right),
                    _backButton.OnClickAsObservable().Select(_ => Vector3.back),
                    _leftButton.OnClickAsObservable().Select(_ => Vector3.left)
                )
                .Subscribe(value => _moveDirection.SetValueAndForceNotify(value));

            //キーが押されたら、フラグを設定する
            Observable.Merge(
                    _undoButton.OnPointerDownAsObservable().Select(_ => true),
                    _undoButton.OnPointerUpAsObservable().Select(_ => false)
                )
                .DistinctUntilChanged()
                .Subscribe(value => _undo.Value = value);
        }
    }
}