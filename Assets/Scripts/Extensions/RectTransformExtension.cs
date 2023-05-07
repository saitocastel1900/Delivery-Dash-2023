using UnityEngine;

namespace Commons.Extensions
{
    public static class RectTransformExtension
    {
        /// <summary>
        /// 位置情報(X)を取得
        /// </summary>
        public static float GetAnchoredPosX(this RectTransform self)
        {
            return self.anchoredPosition.x;
        }

        /// <summary>
        /// 拡縮を設定
        /// </summary>
        public static void SetLocalScaleXY(this Transform self, float xy)
        {
            Vector3 scale = self.localScale;
            scale.x = xy;
            scale.y = xy;
            self.localScale = scale;
        }
    }
}