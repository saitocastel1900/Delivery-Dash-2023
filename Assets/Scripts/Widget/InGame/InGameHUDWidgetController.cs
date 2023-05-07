using Commons.Save;
using UnityEngine;
using Widget.InGame.StageNumber;
using Zenject;

namespace Widget.InGame
{
    public class InGameHUDWidgetController : MonoBehaviour
    {
        /// <summary>
        /// StageNumber
        /// </summary>
        [Inject] private StageNumberPresenter _stageNumber;
        
        /// <summary>
        /// SaveManager
        /// </summary>
        [Inject] private SaveManager _saveManager;

        private void Start()
        {
            _saveManager.Load();
            _stageNumber.SetStageNumber(_saveManager.Data.CurrentStageNumber);
        }
    }
}