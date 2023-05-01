using Commons.Const;
using UnityEngine;

public static class Vector3Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static Vector3 DisplayPosition(this Vector3 original, Vector3 target)
    {
        return new Vector3
        (
            original.x * InGameConst.TileSize - target.x,
            original.y * InGameConst.TileSize + target.y,
            original.z * -InGameConst.TileSize + target.z
        );
    }
}