using Commons.Audio;
using Commons.Const;
using Commons.Input;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Manager.Title
{
    public class TitleManager : MonoBehaviour
    {
        /// <summary>
        /// AudioManager
        /// </summary>
        [Inject] private AudioManager _audioManager;
        
        /// <summary>
        /// ITitleInputEventProvider
        /// </summary>
        [Inject] private ITitleInputEventProvider _input;

        private void Start()
        {
            //エンターが押されたら、Selectシーンに移動
            _input.IsGameStart.SkipLatestValueOnSubscribe().Where(value => value == true)
                .Subscribe(_ =>
                {
                    _audioManager.PlaySoundEffect(SoundEffect.TitleToSelect);
                    SceneManager.LoadScene(Const.StageSelectSceneName);
                });
            
            _audioManager.PlayBGM();
        }
    }
}