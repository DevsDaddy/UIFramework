using UnityEngine;

namespace DevsDaddy.Shared.UIFramework.Core.Utils
{
    /// <summary>
    /// Destroy Helper
    /// </summary>
    internal static class DestroyHelper
    {
        internal static void Destroy(Object @object) {
#if UNITY_EDITOR
            if (Application.isPlaying) {
                Object.Destroy(@object);
            } else {
                Object.DestroyImmediate(@object);
            }
#else
			Object.Destroy(@object);
#endif
        }
    }
}