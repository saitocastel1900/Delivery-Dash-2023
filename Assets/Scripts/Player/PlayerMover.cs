using Manager.Command;
using UniRx;
using UnityEngine;

namespace Player
{
    public class PlayerMover : BasePlayer, IReceiver
    {
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsMoving => _isMoving;

        BoolReactiveProperty _isMoving = new BoolReactiveProperty();

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialize()
        {
            _input.MoveDirection.Subscribe();
        }

        /// <summary>
        /// 移動
        /// </summary>
        /// <param name="direction">移動方向</param>
        public void Move(Vector3 direction)
        {
            transform.position += direction;
        }
    }
}