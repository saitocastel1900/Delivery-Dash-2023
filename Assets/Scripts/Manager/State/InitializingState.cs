using Manager.Command;
using Manager.Stage;
using UI.InGame;
using UnityEngine;

namespace Manager.State
{
    public class InitializingState : IState
    {
        /// <summary>
        /// InGameManager
        /// </summary>
        readonly InGameManager _inGame;

        /// <summary>
        /// InGameMoveCommandManager
        /// </summary>
        readonly InGameCommandManager _command;

        /// <summary>
        /// StageManager
        /// </summary>
        readonly StageGenerator _stage;

        /// <summary>
        /// InGameUIController
        /// </summary>
        readonly InGameUIController _ui;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="command">行動を管理すクラス</param>
        /// <param name="stage">ステージを管理するクラス</param>
        /// <param name="ui">UI管理クラス</param>
        /// <param name="inGame">ステート管理クラス</param>
        public InitializingState(InGameCommandManager command, StageGenerator stage, InGameUIController ui,
            InGameManager inGame)
        {
            _command = command;
            _stage = stage;
            _ui = ui;
            _inGame = inGame;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Enter()
        {
            Debug.Log("InitializingState Enter");

            _stage.LoadTileData();
            _stage.CreateStage();
            _command.Initialize();
            _ui.SetView(true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            Debug.Log("InitializingState Update");

            if (_command.IsDelivered.Value)
                _inGame.CurrentState.TransitionTo(_inGame.CurrentState.ResultState);
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Exit()
        {
            Debug.Log("InitializingState Exit");
        }
    }
}