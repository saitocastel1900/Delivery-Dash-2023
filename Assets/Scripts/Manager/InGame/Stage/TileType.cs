namespace Manager.Stage
{
    public enum TileType
    {
        /// <summary>
        /// 何も無い
        /// </summary>
        NONE,
        
        /// <summary>
        /// 地面
        /// </summary>
        GROUND, 
        
        /// <summary>
        /// 目的地
        /// </summary>
        TARGET, 
        
        /// <summary>
        /// プレイヤー
        /// </summary>
        PLAYER,

        /// <summary>
        /// ブロック
        /// </summary>
        BLOCK, 

        /// <summary>
        /// プレイヤー（目的地の上）
        /// </summary>
        PLAYER_ON_TARGET, 
    
        /// <summary>
        /// ブロック（目的地の上）
        /// </summary>
        BLOCK_ON_TARGET, 
    }
}