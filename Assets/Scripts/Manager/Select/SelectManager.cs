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

        private void Start()
        {
            _input.IsTransition.SkipLatestValueOnSubscribe().Subscribe(_ => _selectWidget.SceneTransition());
        }
    }
}