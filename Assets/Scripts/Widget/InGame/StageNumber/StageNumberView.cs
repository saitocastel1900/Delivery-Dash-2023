using UnityEngine;
using UnityEngine.UI;

namespace Widget.InGame.StageNumber
{
    public class StageNumberView : MonoBehaviour
    {
        /// <summary>
        /// Text
        /// </summary>
        [SerializeField] private Text _text;

        /// <summary>
        /// 番号を設定
        /// </summary>
        /// <param name="number">番号</param>
        public void SetText(int number)
        {
            _text.text = number.ToString();
        }
    }
}