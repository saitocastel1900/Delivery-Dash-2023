using UnityEngine;

namespace Manager.State
{
    public class ResultState : IState
    {
        /// <summary>
        /// InGameManager
        /// </summary>
        readonly InGameManager _inGame;
        
        /// <summary>
        /// MainUIController
        /// </summary>
        readonly MainUIController _ui;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ui">UI管理クラス</param>
        /// <param name="inGame">ステート管理クラス</param>
        public ResultState(MainUIController ui, InGameManager inGame)
        {
            _ui = ui;
            _inGame = inGame;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Enter()
        {
            Debug.Log("ResultState Enter");
            _ui.SetView(false);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            Debug.Log("ResultState Update");
            _inGame.CurrentState.TransitionTo(_inGame.CurrentState.FinishedState);
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Exit()
        {
            Debug.Log("ResultState Exit");
        }
    }
}