using UnityEngine;
using UnityEngine.UI;

namespace UI.Main.StageNumber
{
    public class StageNumberView : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private Text _text;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        public void SetText(int number)
        {
            _text.text = number.ToString();
        }
    }
}