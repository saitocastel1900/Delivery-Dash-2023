using Manager.Command;
using Manager.Stage;
using Manager.State;
using UI.InGame;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

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
        [SerializeField] private InGameCommandManager _commandManager;

        /// <summary>
        /// StageManager
        /// </summary>
        [SerializeField] private StageGenerator _stageManager;

        /// <summary>
        /// InGameUIController
        /// </summary>
        [SerializeField] private InGameUIController _uiController;

        /// <summary>
        /// IInputEventProvider
        /// </summary>
        [Inject] private IInputEventProvider _input;

        private void Start()
        {
            //初期化
            _currentState = new InGameStateMachine(_commandManager, _stageManager, _uiController, this);
            _currentState.Initialize(_currentState.InitializingState);
            this.UpdateAsObservable().Subscribe(_ => _currentState.Update());

            _input.IsReset.SkipLatestValueOnSubscribe().Subscribe(_=>Reset());
            _input.IsQuit.SkipLatestValueOnSubscribe().Subscribe(_ => Quit());
        }
        
        /// <summary>
        /// リセット
        /// </summary>
        private void Reset()
        {
            SceneManager.LoadScene (SceneManager.GetActiveScene().name);
        }
        
        /// <summary>
        /// ゲームを辞める
        /// </summary>
        private void Quit()
        {
#if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_ANDROID)
    Application.Quit();
#elif (UNITY_WEBGL)
    Application.OpenURL("about:blank");
#endif
        }
    }
}