using UniRx;
using UnityEngine;
using Zenject;

namespace Commons.Input
{
    public class KeyInputProvider : IInputEventProvider ,IInitializable
    {
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyReactiveProperty<Vector3> MoveDirection => _moveDirection;
        private ReactiveProperty<Vector3> _moveDirection = new ReactiveProperty<Vector3>();

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyReactiveProperty<bool> UndoButton => _undo;
        private ReactiveProperty<bool> _undo = new ReactiveProperty<bool>(false);
        
        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            Observable.Merge(
                    Observable.EveryUpdate().Where(_=>UnityEngine.Input.GetKeyDown(KeyCode.UpArrow)).Select(_=>Vector3.forward),
                    Observable.EveryUpdate().Where(_=>UnityEngine.Input.GetKeyDown(KeyCode.RightArrow)).Select(_=>Vector3.right),
                    Observable.EveryUpdate().Where(_=>UnityEngine.Input.GetKeyDown(KeyCode.DownArrow)).Select(_=>Vector3.back),
                    Observable.EveryUpdate().Where(_=>UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow)).Select(_=>Vector3.left)
                   //Observable.EveryUpdate().Where(_=>!UnityEngine.Input.anyKey).Select(_=>Vector3.zero)
                )
                .Subscribe(value=>_moveDirection.SetValueAndForceNotify(value));
            
            Observable.EveryUpdate()
                .Select(_=>UnityEngine.Input.GetKeyDown(KeyCode.Space))
                .DistinctUntilChanged()
                .Subscribe(value=>_undo.Value=value);
        }
    }
}