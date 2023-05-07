using UniRx;

namespace Commons.Input
{
    public interface ISelectInputEventProvider
    {
        /// <summary>
        /// 右移動が押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsRight { get; }
        
        /// <summary>
        /// 左移動が押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsLeft { get; }
        
        /// <summary>
        /// シーン遷移が押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsTransition { get; }
    }
}