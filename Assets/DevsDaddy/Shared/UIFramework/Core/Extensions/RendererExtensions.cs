using UnityEngine;

namespace DevsDaddy.Shared.UIFramework.Core.Extensions
{
    public static class RendererExtensions
    {
        private static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
            Vector3[] objectCorners = new Vector3[4];
            rectTransform.GetWorldCorners(objectCorners);
 
            int visibleCorners = 0;
            Vector3 tempScreenSpaceCorner; // Cached
            for (var i = 0; i < objectCorners.Length; i++) // For each corner in rectTransform
            {
                tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]); // Transform world space position of corner to screen space
                if (screenBounds.Contains(tempScreenSpaceCorner)) // If the corner is inside the screen
                {
                    visibleCorners++;
                }
            }
            return visibleCorners;
        }
        
        /// <summary>
        /// Is Full Visible
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera = null) {
            camera = camera == null ? Camera.main : camera;
            
            // Check Active in Hierarchy
            if (!rectTransform.gameObject.activeInHierarchy)
                return false;
            
            return CountCornersVisibleFrom(rectTransform, camera) == 4;
        }
        
        /// <summary>
        /// Is Visible
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera = null)
        {
            camera = camera == null ? Camera.main : camera;
            
            // Check Active in Hierarchy
            if (!rectTransform.gameObject.activeInHierarchy)
                return false;
            
            return CountCornersVisibleFrom(rectTransform, camera) > 0;
        }
    }
}