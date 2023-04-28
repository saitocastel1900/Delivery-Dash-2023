using UnityEngine;

namespace UI.InGame
{
    public class InGameUIController : MonoBehaviour
    {
        /// <summary>
        /// OperationButtons
        /// </summary>
        [SerializeField] private GameObject _operationButtons;

        /// <summary>
        /// StageNumber
        /// </summary>
        [SerializeField] private GameObject _stageNumber;

        /// <summary>
        /// 表示を設定
        /// </summary>
        public void SetView(bool isShow)
        {
            _operationButtons.SetActive(isShow);
            _stageNumber.gameObject.SetActive(isShow);
        }
    }
}