using DevsDaddy.Shared.EventFramework;
using DevsDaddy.Shared.UIFramework.Core;
using DevsDaddy.Shared.UIFramework.Core.Components;
using DevsDaddy.Shared.UIFramework.Payloads;
using UnityEngine;

namespace DevsDaddy.Shared.UIFramework.Demo.Scripts
{
    /// <summary>
    /// Demonstration Home View
    /// </summary>
    internal class DemoHomeView : BaseView
    {
        [Header("Button References")] 
        [SerializeField] private IconTextButton demoPageButton;
        [SerializeField] private IconTextButton quitDialogueButton;

        [Header("Pages Content")] 
        [SerializeField] private GameObject pageContent;

        /// <summary>
        /// On Home View Started
        /// </summary>
        public override void OnViewStart() {
            // On Demo Page Clicked
            demoPageButton.OnClick = () => {
                EventMessenger.Main.Publish(new OnViewNavigated {
                    View = typeof(DemoPageView),
                    Display = new DisplayOptions { IsAnimated = true, Delay = 0f, Duration = 0.5f, Type = AnimationType.Fade },
                    Data = new DemoPageView.Data {
                        Title = "Demo Page",
                        Content = pageContent
                    }
                });
            };
            
            // On Quit Button Clicked
            quitDialogueButton.OnClick = () => {
                EventMessenger.Main.Publish(new OnViewNavigated {
                    View = typeof(DemoConfirmView),
                    Display = new DisplayOptions { IsAnimated = true, Delay = 0f, Duration = 0.5f, Type = AnimationType.Fade },
                    Data = new DemoConfirmView.Data {
                        Title = "Are you Sure?",
                        Message = "Are you sure want to quit from application?",
                        OnApply = Application.Quit,
                        OnCancel = () => {
                            Debug.Log("Application Quit Canceled");
                        },
                        ShowCancel = true
                    }
                });
            };
        }
    }
}