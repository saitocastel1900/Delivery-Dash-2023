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
        /// Resetボタン
        /// </summary>
        private Button _resetButton;
        
        /// <summary>
        /// Quitボタン
        /// </summary>
        private Button _quitButton;
        
        /// <summary>
        /// Spaceボタン
        /// </summary>
        private Button _spaceButton;

        /// <summary>
        /// 進行方向
        /// </summary>
        public IReadOnlyReactiveProperty<Vector3> MoveDirection => _moveDirection;
        private ReactiveProperty<Vector3> _moveDirection = new ReactiveProperty<Vector3>();

        /// <summary>
        /// Undoが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsUndo => _isUndo;
        private ReactiveProperty<bool> _isUndo = new ReactiveProperty<bool>();
        
        /// <summary>
        /// Resetが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsReset => _isReset;
        private ReactiveProperty<bool> _isReset = new ReactiveProperty<bool>(false);
        
        /// <summary>
        /// Quitが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsQuit => _isQuit;
        private ReactiveProperty<bool> _isQuit = new ReactiveProperty<bool>(false);
        
        /// <summary>
        /// Skipが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsSkip => _isSkip;
        private ReactiveProperty<bool> _isSkip = new ReactiveProperty<bool>(false);
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ButtonInputProvider(Button aheadButton, Button leftButton, Button rightButton, Button backButton,
            Button undoButton,Button resetButton,Button quitButton,Button spaceButton)
        {
            _aheadButton = aheadButton;
            _leftButton = leftButton;
            _rightButton = rightButton;
            _backButton = backButton;
            _undoButton = undoButton;
            _resetButton = resetButton;
            _quitButton = quitButton;
            _spaceButton = spaceButton;
        }

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
                .Subscribe(value => _isUndo.Value = value);
            
            //キーが押されたら、フラグを設定する
            Observable.Merge(
                    _resetButton.OnPointerDownAsObservable().Select(_ => true),
                    _resetButton.OnPointerUpAsObservable().Select(_ => false)
                )
                .DistinctUntilChanged()
                .Subscribe(value => _isReset.Value = value);
            
            //キーが押されたら、フラグを設定する
            Observable.Merge(
                    _quitButton.OnPointerDownAsObservable().Select(_ => true),
                    _quitButton.OnPointerUpAsObservable().Select(_ => false)
                )
                .DistinctUntilChanged()
                .Subscribe(value => _isQuit.Value = value);
            
            //キーが押されたら、フラグを設定する
            Observable.Merge(
                    _spaceButton.OnPointerDownAsObservable().Select(_ => true),
                    _spaceButton.OnPointerUpAsObservable().Select(_ => false)
                )
                .DistinctUntilChanged()
                .Subscribe(value => _isQuit.Value = value);
        }
    }
}