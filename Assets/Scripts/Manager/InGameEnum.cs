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
            None = 0,
            Initialize = 1,
            Result = 1 << 1,
            Finished = 1 << 2,
        }
    }
}