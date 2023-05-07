using System;

namespace Commons.Save
{
    [Serializable]
    public class SaveData
    {
        /// <summary>
        /// クリアした最大のステージ番号
        /// </summary>
        public int MaxStageClearNumber = 0;
        
        /// <summary>
        /// 現在のステージ番号
        /// </summary>
        public int CurrentStageNumber = 0;
    }
}
