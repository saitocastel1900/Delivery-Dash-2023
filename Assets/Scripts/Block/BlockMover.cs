using Manager.Command;
using UnityEngine;

namespace Block
{
    public class BlockMover : MonoBehaviour, IReceiver
    {
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