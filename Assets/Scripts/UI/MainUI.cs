using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private MoveCommandPresenter _moveButton;
    [SerializeField] private StageNumber _stageNumber;
    
    private void Start()
    {
        _moveButton.Initialize();
        _stageNumber.Initialize();
    }
}
