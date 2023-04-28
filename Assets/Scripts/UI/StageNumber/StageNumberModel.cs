using UniRx;

namespace UI.InGame.StageNumber
{
    public class StageNumberModel : IStageNumberModel
    {
        /// <summary>
        /// ステージ番号
        /// </summary>
        public IReactiveProperty<int> StageNumberProp => _stageNumberProp;
        private IntReactiveProperty _stageNumberProp;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StageNumberModel()
        {
            _stageNumberProp = new IntReactiveProperty(01);
        }
        
        /// <summary>
        /// ステージ番号を設定
        /// </summary>
        /// <param name="number">番号</param>
        public void SetStageNumber(int number)
        {
            _stageNumberProp.Value = number;
        }
    }
}