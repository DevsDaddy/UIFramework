using System;
using UnityEngine;

namespace DevsDaddy.Shared.UIFramework.Demo.Scripts
{
    /// <summary>
    /// UI Framework Demo Scene Installer
    ///
    /// Documentation:
    /// https://github.com/DevsDaddy/UIFramework/wiki
    /// </summary>
    internal sealed class SceneInstaller : MonoBehaviour
    {
        [Header("View References")] 
        [SerializeField] private DemoHomeView homeView;
        [SerializeField] private DemoPageView pageView;
        [SerializeField] private DemoConfirmView confirmView;

        /// <summary>
        /// On Scene Installer Start
        /// </summary>
        private void Start() {
            BindViews();
        }
        
        /// <summary>
        /// Bind Scene Views
        /// </summary>
        private void BindViews() {
            UIFramework.BindView(Instantiate(homeView), true);
            UIFramework.BindView(Instantiate(pageView));
            UIFramework.BindView(Instantiate(confirmView));
        }
    }
}