using Commons.Const;
using Commons.Save;
using Commons.Utility;
using UnityEngine.SceneManagement;

namespace Manager.InGame.State
{
    public class FinishedState : IState
    {
        /// <summary>
        /// SaveManager
        /// </summary>
        /// <returns></returns>
        private readonly SaveManager _saveManager;

        /// <summary>
        /// InGameManager
        /// </summary>
        readonly InGameManager _inGame;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="saveManager">セーブシステム</param>
        /// <param name="inGame">ステート管理クラス</param>
        public FinishedState(SaveManager saveManager, InGameManager inGame)
        {
            _saveManager = saveManager;
            _inGame = inGame;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Enter()
        {
            DebugUtility.Log("FinishedState Enter");
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            DebugUtility.Log("FinishedState Update");

            _saveManager.Load();
            
            if (_saveManager.Data.CurrentStageNumber + 1 < Const.StagesNumber)
            {
                _saveManager.Data.CurrentStageNumber++;
            }

            if (_saveManager.Data.CurrentStageNumber > _saveManager.Data.MaxStageClearNumber &&
                _saveManager.Data.MaxStageClearNumber + 1 < Const.StagesNumber)
            {
                _saveManager.Data.MaxStageClearNumber++;
            }

            _saveManager.Save();

            SceneManager.LoadScene("Stage" + _saveManager.Data.CurrentStageNumber);
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Exit()
        {
            DebugUtility.Log("FinishedState Exit");
        }
    }
}