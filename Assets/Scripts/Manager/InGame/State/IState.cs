namespace Manager.InGame.State
{
    public interface IState
    {
        /// <summary>
        /// 初期化
        /// </summary>
        public void Enter();

        /// <summary>
        /// 更新
        /// </summary>
        public void Update();

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Exit();
    }
}