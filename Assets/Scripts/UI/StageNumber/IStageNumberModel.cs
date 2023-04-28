using UniRx;

namespace UI.InGame.StageNumber
{
    public interface IStageNumberModel
    {
        /// <summary>
        /// ステージ番号
        /// </summary>
        public IReactiveProperty<int> StageNumberProp { get; }

        /// <summary>
        /// ステージ番号を設定
        /// </summary>
        /// <param name="number">番号</param>
        public void SetStageNumber(int number);
    }
}