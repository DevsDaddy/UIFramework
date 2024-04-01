using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevsDaddy.Shared.UIFramework.Core.Components
{
    /// <summary>
    /// Base Icon Button with Text
    /// </summary>
    [AddComponentMenu("UIFramework/Components/Icon Text Button")]
    [RequireComponent(typeof(Button))]
    public sealed class IconTextButton : MonoBehaviour
    {
        [Header("Button Data")] 
        [SerializeField] private Sprite defaultIcon;
        [SerializeField] private string defaultText = "Button";
        
        [Header("Button References")] 
        [SerializeField] private Button button;
        [SerializeField] private Image iconContainer;
        [SerializeField] private TextMeshProUGUI textContainer;
        
        public Action OnClick;
        
        /// <summary>
        /// On Awake
        /// </summary>
        private void Awake() {
            // Get Icon Container
            if (iconContainer == null) {
                iconContainer = GetComponentInChildren<Image>();
                if(iconContainer == null) {
                    Debug.LogWarning($"No icon container found for Button {this.name}");
                    iconContainer.gameObject.SetActive(false);
                }
            }
            
            // Get Text Container
            if (textContainer == null) {
                textContainer = GetComponentInChildren<TextMeshProUGUI>();
                if(textContainer == null) {
                    Debug.LogWarning($"No text container found for Button {this.name}");
                    textContainer.gameObject.SetActive(false);
                }
            }

            // Setup Button
            if (button == null) {
                button = GetComponent<Button>();
            }
        }

        /// <summary>
        /// On Start
        /// </summary>
        private void Start() {
            // Setup Icon
            if(iconContainer != null && defaultIcon != null)
                iconContainer.sprite = defaultIcon;
            
            // Setup Default Text
            if(textContainer != null)
                textContainer.SetText(defaultText);

            // Setup Handler
            UpdateButtonHandler();
        }

        /// <summary>
        /// On Destroy
        /// </summary>
        private void OnDestroy() {
            button.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// Update Button Handler
        /// </summary>
        private void UpdateButtonHandler() {
            // Setup Button Handler
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=> OnClick?.Invoke());
        }

        /// <summary>
        /// Update Button
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="icon"></param>
        /// <param name="onClick"></param>
        public void UpdateButton(string buttonText, Sprite icon = null, Action onClick = null) {
            if (iconContainer != null && icon != null)
                iconContainer.sprite = icon;
            
            if(textContainer != null)
                textContainer.SetText(buttonText);

            OnClick = onClick;
            
            // Setup Handler
            UpdateButtonHandler();
        }
    }
}