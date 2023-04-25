using UniRx;

namespace UI.Main.StageNumber
{
    public class StageNumberModel : IStageNumberModel
    {
        /// <summary>
        /// 
        /// </summary>
        public IReactiveProperty<int> StageNumberProp => _stageNumberProp;
        private IntReactiveProperty _stageNumberProp;

        /// <summary>
        /// 
        /// </summary>
        public StageNumberModel()
        {
            _stageNumberProp = new IntReactiveProperty(01);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        public void SetStageNumber(int number)
        {
            _stageNumberProp.Value = number;
        }
    }
}