using Commons.Enum;
using UnityEngine;


public class InGameManager : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private InGameMoveCommandManager _commandManager;

    [SerializeField] private StageManager _stageCreateManager;
    
    
    //TODO:単純に初期化で色々な処理をしているので、細かく分ける
    private void Start()
    {
        _stageCreateManager.LoadTileData();
        _stageCreateManager.CreateStage();
        
        _commandManager.Initialize();
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnStageChanged(InGameEnum.State state)
    {
        switch (state)
        {
            
        }
    }
}
