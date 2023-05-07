using Commons.Utility;
using Manager.Command;
using Manager.Stage;
using UnityEngine;
using Widget.Result;

namespace Manager.InGame.State
{
    public class InitializingState : IState
    {
        /// <summary>
        /// InGameMoveCommandManager
        /// </summary>
        readonly InGameCommandManager _command;

        /// <summary>
        /// StageManager
        /// </summary>
        readonly StageGenerator _stage;
        
        /// <summary>
        /// 
        /// </summary>
        readonly ResultWidgetController _resultWidget;
        
        /// <summary>
        /// InGameManager
        /// </summary>
        readonly InGameManager _inGame;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="command">行動を管理すクラス</param>
        /// <param name="stage">ステージを管理するクラス</param>
        /// <param name="resultWidget"></param>
        /// <param name="inGame">ステート管理クラス</param>
        public InitializingState(InGameCommandManager command, StageGenerator stage, ResultWidgetController resultWidget,InGameManager inGame)
        {
            _command = command;
            _stage = stage;
            _resultWidget = resultWidget;
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
            _resultWidget.SetView(false);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            DebugUtility.Log("InitializingState Update");

            if (_command.IsDelivered.Value)
                _inGame.CurrentState.TransitionTo(_inGame.CurrentState.ResultState);
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Exit()
        {
            DebugUtility.Log("InitializingState Exit");
        }
    }
}