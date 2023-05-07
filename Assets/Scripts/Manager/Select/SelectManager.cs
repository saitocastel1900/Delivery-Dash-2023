using Commons.Audio;
using Commons.Input;
using UniRx;
using UnityEngine;
using Widget.Select;
using Zenject;

namespace Manager.Select
{
    public class SelectManager : MonoBehaviour
    {
        /// <summary>
        /// StageSelectWidgetController
        /// </summary>
        [SerializeField] private StageSelectWidgetController _selectWidget;

        /// <summary>
        /// ISelectInputEventProvider
        /// </summary>
        [Inject] private ISelectInputEventProvider _input;

        /// <summary>
        /// AudioManager
        /// </summary>
        [Inject] private AudioManager _audioManager;

        private void Start()
        {
            _input.IsLeft.SkipLatestValueOnSubscribe().Where(value => value == true)
                .Subscribe(_ => _selectWidget.TurnLeft());
            _input.IsRight.SkipLatestValueOnSubscribe().Where(value => value == true)
                .Subscribe(_ => _selectWidget.TurnRight());
            _input.IsTransition.SkipLatestValueOnSubscribe().Subscribe(_ => _selectWidget.SceneTransition());
        }
    }
}