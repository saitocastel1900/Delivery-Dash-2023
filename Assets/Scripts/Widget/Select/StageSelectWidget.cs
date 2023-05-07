using UnityEngine;
using UnityEngine.UI;

namespace Widget.Select
{
    public class StageSelectWidget : MonoBehaviour
    {
        /// <summary>
        /// Text
        /// </summary>
        [SerializeField] private Text _text1;
        
        /// <summary>
        /// Text
        /// </summary>
        [SerializeField] private Text _text2;

        /// <summary>
        /// インタラクションを設定する
        /// </summary>
        public void SetInteractable(bool isInteractable)
        {
            if (!isInteractable)
            {
                _text1.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                _text2.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }
    }
}