using UnityEngine;

namespace Wedge.InGame
{
    public class InGameHUDWedgeController : MonoBehaviour
    {
        /// <summary>
        /// OperationButtons
        /// </summary>
        [SerializeField] private GameObject _operationButtons;
        
        /// <summary>
        /// OperationPanels
        /// </summary>
        [SerializeField] private GameObject _operationPanels;

        /// <summary>
        /// StageNumber
        /// </summary>
        [SerializeField] private GameObject _stageNumber;

        private void Start()
        {
            _operationPanels.SetActive(false);
            _operationButtons.SetActive(false);
            _stageNumber.gameObject.SetActive(false);
        }

        /// <summary>
        /// 表示を設定
        /// </summary>
        public void SetView(bool isShow)
        {
#if UNITY_EDITOR || UNITY_WEBGL
            _operationPanels.SetActive(isShow);
#elif UNITY_ANDROID
            _operationButtons.SetActive(isShow);
#endif
            _stageNumber.gameObject.SetActive(isShow);
        }
    }
}