using Manager.Command;
using Manager.Stage;
using Wedge.InGame;

namespace Manager.State
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
        /// <param name="command"></param>
        /// <param name="stage"></param>
        /// <param name="ui"></param>
        /// <param name="inGame"></param>
        public InGameStateMachine(InGameCommandManager command, StageGenerator stage, InGameHUDWedgeController ui,
            InGameManager inGame)
        {
            InitializingState = new InitializingState(command, stage, ui, inGame);
            ResultState = new ResultState(ui, inGame);
            FinishedState = new FinishedState(inGame);
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