using System;
using DevsDaddy.Shared.UIFramework.Core;
using DevsDaddy.Shared.UIFramework.Core.Components;
using DevsDaddy.Shared.UIFramework.Payloads;
using TMPro;
using UnityEngine;

namespace DevsDaddy.Shared.UIFramework.Demo.Scripts
{
    /// <summary>
    /// Demo Confirm View
    /// </summary>
    internal class DemoConfirmView : BaseView
    {
        [Header("Confirm View References")] 
        [SerializeField] private TextMeshProUGUI headline;
        [SerializeField] private TextMeshProUGUI message;
        [SerializeField] private IconTextButton confirmButton;
        [SerializeField] private IconTextButton cancelButton;
        
        [System.Serializable]
        public class Data : IViewData
        {
            public string Title = "";
            public string Message = "";
            
            public bool ShowCancel = true;
            public Action OnApply;
            public Action OnCancel;
        }
        private Data currentData;
        
        /// <summary>
        /// On View Start
        /// </summary>
        public override void OnViewStart() {
            confirmButton.OnClick = () => {
                currentData?.OnApply?.Invoke();
                UIFramework.GoBack(new DisplayOptions { IsAnimated = true, Delay = 0f, Duration = 0.5f, Type = AnimationType.Fade });
            };
            cancelButton.OnClick = () => {
                currentData?.OnCancel?.Invoke();
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
            message.SetText(currentData.Message);
            cancelButton.gameObject.SetActive(currentData.ShowCancel);
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