using UniRx;

namespace Commons.Input
{
    public interface ITitleInputEventProvider
    {
        /// <summary>
        /// ゲームスタートが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsGameStart { get; }
    }
}