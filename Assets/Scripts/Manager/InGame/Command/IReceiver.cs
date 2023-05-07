using UnityEngine;

namespace Manager.Command
{
    public interface IReceiver
    {
        /// <summary>
        /// 移動
        /// </summary>
        /// <param name="direction">移動方向</param>
        public void Move(Vector3 direction);
    }
}