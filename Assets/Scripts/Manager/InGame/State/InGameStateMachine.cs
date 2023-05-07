using Commons.Save;
using Manager.Command;
using Manager.Stage;
using Widget.Result;

namespace Manager.InGame.State
{
    public class InGameStateMachine
    {
        /// <summary>
        /// InitializingState
        /// </summary>
        public InitializingState InitializingState;

        /// <summary>
        /// ResultState
        /// </summary>
        public ResultState ResultState;

        /// <summary>
        /// FinishedState
        /// </summary>
        public FinishedState FinishedState;

        /// <summary>
        /// 現在の状態
        /// </summary>
        private IState _currentState;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="command">行動を管理すクラス</param>
        /// <param name="stage">ステージを管理するクラス</param>
        /// <param name="saveManager">セーブシステム</param>
        /// <param name="resultWidget"></param>
        /// <param name="inGame">ステート管理クラス</param>
        public InGameStateMachine(InGameCommandManager command, StageGenerator stage, SaveManager saveManager,ResultWidgetController resultWidget,InGameManager inGame)
        {
            InitializingState = new InitializingState(command, stage, resultWidget,inGame);
            ResultState = new ResultState(resultWidget,inGame);
            FinishedState = new FinishedState(saveManager,inGame);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="state">状態</param>
        public void Initialize(IState state)
        {
            this._currentState = state;
            state.Enter();
        }

        /// <summary>
        /// 遷移
        /// </summary>
        /// <param name="nextState">つぎの状態</param>
        public void TransitionTo(IState nextState)
        {
            _currentState.Exit();
            _currentState = nextState;
            nextState.Enter();
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            _currentState?.Update();
        }
    }
}