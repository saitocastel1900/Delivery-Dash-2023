using UniRx;
using UnityEngine;
using Zenject;

namespace Commons.Input
{
    public class KeyInputProvider : IInputEventProvider, IInitializable
    {
        /// <summary>
        /// 進行方向
        /// </summary>
        public IReadOnlyReactiveProperty<Vector3> MoveDirection => _moveDirection;
        private ReactiveProperty<Vector3> _moveDirection = new ReactiveProperty<Vector3>();

        /// <summary>
        /// Undoボタンが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> UndoButton => _undo;
        private ReactiveProperty<bool> _undo = new ReactiveProperty<bool>(false);

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            //キーが押されたら、進行方向を設定する
            Observable.Merge(
                    Observable.EveryUpdate().Where(_ => UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
                        .Select(_ => Vector3.forward),
                    Observable.EveryUpdate().Where(_ => UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
                        .Select(_ => Vector3.right),
                    Observable.EveryUpdate().Where(_ => UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
                        .Select(_ => Vector3.back),
                    Observable.EveryUpdate().Where(_ => UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
                        .Select(_ => Vector3.left)
                )
                .Subscribe(value => _moveDirection.SetValueAndForceNotify(value));

            //キーが押されたら、フラグを設定する
            Observable.Merge(
                    Observable.EveryUpdate().Where(_ => UnityEngine.Input.GetKeyDown(KeyCode.Space)).Select(_ => true),
                    Observable.EveryUpdate().Where(_ => UnityEngine.Input.GetKeyUp(KeyCode.Space)).Select(_ => false)
                )
                .DistinctUntilChanged()
                .Subscribe(value => _undo.Value = value);
        }
    }
}