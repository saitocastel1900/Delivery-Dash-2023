using UI.Main.StageNumber;
using UnityEngine;
using Zenject;

namespace UI.Main
{
    public class MainUI : MonoBehaviour
    {
        [Inject] private StageNumberPresenter _stageNumber;

        private void Start()
        {

        }
    }
}