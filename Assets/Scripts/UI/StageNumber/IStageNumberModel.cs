using UniRx;

namespace UI.Main.StageNumber
{
    public interface IStageNumberModel
    {
        /// <summary>
        /// 
        /// </summary>
        public IReactiveProperty<int> StageNumberProp { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public void SetStageNumber(int number);
    }
}