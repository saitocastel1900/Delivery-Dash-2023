using Commons.Save;
using UniRx;
using Zenject;

namespace Widget.InGame.StageNumber
{
    public class StageNumberModel : IStageNumberModel , IInitializable
    {
        /// <summary>
        /// ステージ番号
        /// </summary>
        public IReactiveProperty<int> StageNumberProp => _stageNumberProp;
        private IntReactiveProperty _stageNumberProp;

        /// <summary>
        /// 
        /// </summary>
        [Inject] private SaveManager _saveManager;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StageNumberModel()
        {
            _stageNumberProp = new IntReactiveProperty(0);
        }

        public void Initialize()
        {
            _saveManager.Load();
            _stageNumberProp.Value = _saveManager.Data.CurrentStageNumber;
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