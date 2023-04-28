using UnityEngine;

namespace Manager.Command
{
    public class MoveCommand : ICommand
    {
        /// <summary>
        /// 実行される命令
        /// </summary>
        private IReceiver _receiver;
        
        /// <summary>
        /// 移動方向
        /// </summary>
        private Vector3 _direction;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="receiver">実行される命令</param>
        /// <param name="direction">移動方向</param>
        public MoveCommand(IReceiver receiver, Vector3 direction)
        {
            _receiver = receiver;
            _direction = direction;
        }

        /// <summary>
        /// 実行
        /// </summary>
        public void Execute()
        {
            _receiver.Move(_direction);
        }

        /// <summary>
        /// 実行巻き戻し
        /// </summary>
        public void Undo()
        {
            _receiver.Move(-_direction);
        }
    }
}