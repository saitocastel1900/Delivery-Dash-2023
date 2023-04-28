using Manager.Command;
using Manager.State;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Manager
{
    public class InGameManager : MonoBehaviour
    {
        /// <summary>
        /// インゲームの状態
        /// </summary>
        public InGameStateMachine CurrentState => _currentState;
        private InGameStateMachine _currentState;

        /// <summary>
        /// InGameMoveCommandManager
        /// </summary>
        [SerializeField] private InGameMoveCommandManager _commandManager;

        /// <summary>
        /// StageManager
        /// </summary>
        [SerializeField] private StageManager _stageManager;

        /// <summary>
        /// UI
        /// </summary>
        [SerializeField] private MainUIController _uiController;

        private void Start()
        {
            //初期化
            _currentState = new InGameStateMachine(_commandManager, _stageManager, _uiController, this);
            _currentState.Initialize(_currentState.InitializingState);
            this.UpdateAsObservable().Subscribe(_ => _currentState.Update());
        }
    }
}