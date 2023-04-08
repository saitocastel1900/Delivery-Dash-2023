using UnityEngine.UI;
using UnityEngine;

public class StageNumber : MonoBehaviour
{
    [SerializeField] private Text _stageNumber;
    
    public void Initialize()
    {
        _stageNumber.text = " ";
    }
}
