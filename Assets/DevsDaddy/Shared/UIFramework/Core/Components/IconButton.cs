using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevsDaddy.Shared.UIFramework.Core.Components
{
    /// <summary>
    /// Base Icon Button
    /// </summary>
    [AddComponentMenu("UIFramework/Components/Icon Button")]
    [RequireComponent(typeof(Button))]
    public sealed class IconButton : MonoBehaviour
    {
        [Header("Button Data")] 
        [SerializeField] private Sprite defaultIcon;
        
        [Header("Button References")] 
        [SerializeField] private Button button;
        [SerializeField] private Image iconContainer;

        public Action OnClick;

        /// <summary>
        /// On Awake
        /// </summary>
        private void Awake() {
            // Get Icon Container
            if (iconContainer == null) {
                iconContainer = GetComponentInChildren<Image>();
                if(iconContainer == null) Debug.LogWarning($"No icon container found for Button {this.name}");
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
        /// <param name="icon"></param>
        /// <param name="onClick"></param>
        public void UpdateButton(Sprite icon, Action onClick = null) {
            if (iconContainer != null)
                iconContainer.sprite = icon;
            else
                Debug.LogWarning($"Failed to set {icon.name} to Button {this.name}, because for this button is not found image container.");

            OnClick = onClick;
            
            // Setup Handler
            UpdateButtonHandler();
        }
    }
}