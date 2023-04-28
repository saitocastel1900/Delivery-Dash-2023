using UnityEngine;

namespace Manager.State
{
    public class FinishedState : IState
    {
        /// <summary>
        /// InGameManager
        /// </summary>
        readonly InGameManager _inGame;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="inGame">ステート管理クラス</param>
        public FinishedState(InGameManager inGame)
        {
            _inGame = inGame;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Enter()
        {
            Debug.Log("FinishedState Enter");
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            Debug.Log("FinishedState Update");
            _inGame.CurrentState.TransitionTo(_inGame.CurrentState.InitializingState);
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Exit()
        {
            Debug.Log("FinishedState Exit");
        }
    }
}