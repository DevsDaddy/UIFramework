using System;
using DevsDaddy.Shared.EventFramework;
using DevsDaddy.Shared.UIFramework.Core;
using DevsDaddy.Shared.UIFramework.Core.Components;
using DevsDaddy.Shared.UIFramework.Payloads;
using TMPro;
using UnityEngine;

namespace DevsDaddy.Shared.UIFramework.Demo.Scripts
{
    /// <summary>
    /// Demo Page View
    /// </summary>
    internal class DemoPageView : BaseView
    {
        [Header("Page View References")] 
        [SerializeField] private IconButton backButton;
        [SerializeField] private TextMeshProUGUI headline;
        [SerializeField] private RectTransform container;
        
        [System.Serializable]
        public class Data : IViewData
        {
            public string Title = "";
            public GameObject Content;
        }
        private Data currentData;

        /// <summary>
        /// On View Start
        /// </summary>
        public override void OnViewStart() {
            backButton.OnClick = () => {
                UIFramework.GoBack(new DisplayOptions { IsAnimated = true, Delay = 0f, Duration = 0.5f, Type = AnimationType.Fade });
            };
        }

        /// <summary>
        /// On Data Updated
        /// </summary>
        /// <param name="updated"></param>
        public override void OnDataUpdated(OnViewUpdated updated) {
            if(updated.View != GetType()) return;
            currentData = (Data)updated.Data ?? throw new Exception($"Failed to show view: {GetType()}. No data found in payload");
            headline.SetText(currentData.Title);
            foreach (Transform child in container.transform) {
                Destroy(child.gameObject);
            }

            Instantiate(currentData.Content, container.transform);
        }

        /// <summary>
        /// On View Navigated
        /// </summary>
        /// <param name="payload"></param>
        public override void OnNavigated(OnViewNavigated payload) {
            if(payload.View != GetType()) return;
            OnDataUpdated(new OnViewUpdated{ Data = payload.Data, View = GetType() });
            UIFramework.Navigate(this, payload.Display);
        }
    }
}