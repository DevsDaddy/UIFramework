namespace DevsDaddy.Shared.UIFramework.Core
{
    /// <summary>
    /// Base View Interface
    /// </summary>
    public interface IBaseView
    {
        /// <summary>
        /// Set View as Interscenic Object
        /// </summary>
        void SetAsGlobalView();

        /// <summary>
        /// Show View
        /// </summary>
        /// <param name="options"></param>
        void ShowView(DisplayOptions options = null);

        /// <summary>
        /// Hide View
        /// </summary>
        /// <param name="options"></param>
        void HideView(DisplayOptions options = null);

        /// <summary>
        /// Toggle View
        /// </summary>
        /// <param name="options"></param>
        void ToggleView(DisplayOptions options = null);

        /// <summary>
        /// Is View is Visible Now
        /// </summary>
        /// <returns></returns>
        bool IsVisible();

        // Lifetime Virtual Methods
        void OnViewAwake();
        void OnViewStart();
        void OnViewDestroy();
    }
}