using UniRx;
using UnityEngine;

namespace Commons.Input
{
    public interface IInGameInputEventProvider
    {
        /// <summary>
        /// 進行方向
        /// </summary>
        public IReadOnlyReactiveProperty<Vector3> MoveDirection { get; }

        /// <summary>
        /// Undoが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsUndo { get; }

        /// <summary>
        /// Resetが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsReset { get; }

        /// <summary>
        /// Quitが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsQuit { get; }

        /// <summary>
        /// Skipが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsSkip { get; }
    }
}