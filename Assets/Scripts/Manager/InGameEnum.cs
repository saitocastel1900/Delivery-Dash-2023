using System;

namespace Commons.Enum
{
    public static class InGameEnum
    {
        /// <summary>
        /// 状態
        /// </summary>
        [Flags]
        public enum State
        {
            /// <summary>
            /// 何もしない
            /// </summary>
            None = 0,
            
            /// <summary>
            /// 初期化
            /// </summary>
            Initialize = 1,
            
            /// <summary>
            /// リザルト
            /// </summary>
            Result = 1 << 1,
            
            /// <summary>
            /// ゲーム終了
            /// </summary>
            Finished = 1 << 2,
        }
    }
}