using System;

namespace DevsDaddy.Shared.UIFramework.Core
{
    [Serializable]
    public class DisplayOptions
    {
        public bool IsAnimated = false;
        public float Delay = 0f;
        public float Duration = 0.5f;
        public AnimationType Type = AnimationType.Fade;
        public Action<bool> OnComplete = null;
    }
}