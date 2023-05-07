using Commons.Audio;
using Commons.Const;
using Commons.Input;
using Commons.Save;
using Manager.Command;
using Manager.InGame.State;
using Manager.Stage;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Widget.Result;
using Zenject;

namespace Manager.InGame
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
        [SerializeField] private StageGenerator _stageGenerator;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private ResultWidgetController _resultWidget;
        
        /// <summary>
        /// IInputEventProvider
        /// </summary>
        [Inject] private IInGameInputEventProvider _input;
        
        /// <summary>
        /// 
        /// </summary>
        [Inject] private AudioManager _audioManager;
        
        /// <summary>
        /// 
        /// </summary>
        [Inject] private SaveManager _saveManager;

        private void Start()
        {
            //初期化
            _currentState = new InGameStateMachine(_commandManager, _stageGenerator, _saveManager,_resultWidget,this);
            _currentState.Initialize(_currentState.InitializingState);
            
            this.UpdateAsObservable().Subscribe(_ => _currentState.Update());
            
            _input.IsReset.SkipLatestValueOnSubscribe().Subscribe(_=>
            {
                _audioManager.PlaySoundEffect(SoundEffect.Reset);
                Reset();
            });
            _input.IsQuit.SkipLatestValueOnSubscribe().Subscribe(_ =>
            {
                _audioManager.PlaySoundEffect(SoundEffect.Quit);
                Quit();
            });
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
            SceneManager.LoadScene (Const.StageSelectSceneName);
        }
    }
}