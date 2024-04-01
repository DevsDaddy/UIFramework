using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevsDaddy.Shared.EventFramework;
using DevsDaddy.Shared.UIFramework.Payloads;
using UnityEngine;

namespace DevsDaddy.Shared.UIFramework.Core
{
    /// <summary>
    /// Base UI View
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class BaseView : MonoBehaviour, IBaseView
    {
        [Header("View Options")] 
        public bool InitialVisibility = false;

        // Display Options and Components
        private Canvas m_CurrentCanvas;
        private CanvasGroup m_CurrentGroup;
        private RectTransform m_CurrentTransform;
        private DisplayOptions m_CurrentDisplayOptions = new DisplayOptions();
        
        // Delegates
        public delegate void ViewShown();
        public ViewShown OnViewShown;
        
        public delegate void ViewHidden();
        public ViewHidden OnViewHidden;
        
        public delegate void ViewUpdated();
        public ViewUpdated OnViewUpdated;
        
        // Current Visibility
        private bool isVisible = false;

        #region General
        /// <summary>
        /// On View Awake
        /// </summary>
        private void Awake() {
            m_CurrentCanvas = GetComponent<Canvas>();
            m_CurrentGroup = GetComponent<CanvasGroup>();
            m_CurrentTransform = GetComponent<RectTransform>();
            BindEvents();
            OnViewAwake();
        }
        public virtual void OnViewAwake(){}

        /// <summary>
        /// On View Start
        /// </summary>
        private void Start() {
            if(!InitialVisibility)
                HideView(new DisplayOptions { IsAnimated = false, OnComplete = visibility => OnViewHidden?.Invoke() });
            else
                ShowView(new DisplayOptions { IsAnimated = false, OnComplete = visibility => OnViewShown?.Invoke() });
            
            OnViewStart();
        }
        public virtual void OnViewStart(){}

        /// <summary>
        /// On View Destroy
        /// </summary>
        private void OnDestroy() {
            UnbindEvents();
            OnViewDestroy();
        }
        public virtual void OnViewDestroy(){}
        #endregion
        
        #region Base View Actions
        /// <summary>
        /// Set as Global View
        /// </summary>
        public void SetAsGlobalView() {
            transform.SetParent(null);
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Show View
        /// </summary>
        /// <param name="options"></param>
        public void ShowView(DisplayOptions options = null) {
            Debug.Log($"View Show {this.GetType().Name} is requested");
            StopCoroutine(nameof(AnimateView));
            if(options != null) SetDisplayOptions(options);
            if (m_CurrentDisplayOptions.IsAnimated) {
                StartCoroutine(AnimateView(true, () => {
                    m_CurrentDisplayOptions.OnComplete?.Invoke(true);
                    OnViewShown?.Invoke();
                    Debug.Log($"View {this.GetType().Name} is shown");
                }));
            }
            else {
                SetVisibility(true);
                m_CurrentDisplayOptions.OnComplete?.Invoke(true);
                OnViewShown?.Invoke();
                Debug.Log($"View {this.GetType().Name} is shown");
            }
        }

        /// <summary>
        /// Hide View
        /// </summary>
        /// <param name="options"></param>
        public void HideView(DisplayOptions options = null) {
            Debug.Log($"View Hide {this.GetType().Name} is requested");
            StopCoroutine(nameof(AnimateView));
            if(options != null) SetDisplayOptions(options);
            if (m_CurrentDisplayOptions.IsAnimated) {
                StartCoroutine(AnimateView(false, () => {
                    m_CurrentDisplayOptions.OnComplete?.Invoke(false);
                    OnViewHidden?.Invoke();
                    Debug.Log($"View {this.GetType().Name} is hidden");
                }));
            }
            else {
                SetVisibility(false);
                m_CurrentDisplayOptions.OnComplete?.Invoke(false);
                OnViewHidden?.Invoke();
                Debug.Log($"View {this.GetType().Name} is hidden");
            }
        }

        /// <summary>
        /// Toggle View
        /// </summary>
        /// <param name="options"></param>
        public void ToggleView(DisplayOptions options = null) {
            if(isVisible)
                HideView(options);
            else
                ShowView(options);
        }

        /// <summary>
        /// Set Current Display Options
        /// </summary>
        /// <param name="options"></param>
        public void SetDisplayOptions(DisplayOptions options) {
            m_CurrentDisplayOptions = options;
        }

        /// <summary>
        /// Is View is Visible now
        /// </summary>
        /// <returns></returns>
        public bool IsVisible() {
            return isVisible;
        }

        /// <summary>
        /// Animate View
        /// </summary>
        /// <param name="visible"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        private IEnumerator AnimateView(bool visible, Action onComplete = null) {
            // Setup for changing
            Debug.Log($"Start Animating View {this.GetType()}");
            m_CurrentCanvas.enabled = true;
            m_CurrentGroup.alpha = (m_CurrentDisplayOptions.Type == AnimationType.Fade) ? !visible ? 1f : 0f : 1f;
            m_CurrentGroup.blocksRaycasts = !visible;
            m_CurrentGroup.interactable = !visible;
            m_CurrentTransform.localScale = (m_CurrentDisplayOptions.Type == AnimationType.Scale) ? !visible ? Vector3.one : Vector3.zero : Vector3.one;

            float initialAlpha = m_CurrentGroup.alpha;
            Vector3 initialSize = m_CurrentTransform.localScale;

            // Delay
            if (m_CurrentDisplayOptions.Delay > 0f) {
                yield return new WaitForSeconds(m_CurrentDisplayOptions.Delay);
            }
            
            // Animate
            float elapsedTime = 0f;
            while (elapsedTime < m_CurrentDisplayOptions.Duration) {
                elapsedTime += Time.deltaTime;
                if (m_CurrentDisplayOptions.Type == AnimationType.Fade) {
                    m_CurrentGroup.alpha = Mathf.Lerp(initialAlpha, visible ? 1f : 0f,
                        elapsedTime / m_CurrentDisplayOptions.Duration);
                }else if (m_CurrentDisplayOptions.Type == AnimationType.Scale) {
                    m_CurrentTransform.localScale = Vector3.Lerp(initialSize, visible ? Vector3.one : Vector3.zero,
                        elapsedTime / m_CurrentDisplayOptions.Duration);
                }
                
                yield return null;
            }
            
            isVisible = visible;
            m_CurrentGroup.blocksRaycasts = isVisible;
            m_CurrentGroup.interactable = isVisible;
            m_CurrentCanvas.enabled = isVisible;
            onComplete?.Invoke();
                
            if (isVisible)
                EventMessenger.Main.Publish(new OnViewShown { View = this });
            else
                EventMessenger.Main.Publish(new OnViewHidden { View = this });
            
            Debug.Log($"End Animating View {this.GetType()}");
        }

        /// <summary>
        /// Set Visibility without animation
        /// </summary>
        /// <param name="visible"></param>
        private void SetVisibility(bool visible) {
            isVisible = visible;
            m_CurrentTransform.localScale = isVisible ? Vector3.one : Vector3.zero;
            m_CurrentGroup.alpha = isVisible ? 1f : 0f;
            m_CurrentGroup.blocksRaycasts = isVisible;
            m_CurrentGroup.interactable = isVisible;
            m_CurrentCanvas.enabled = isVisible;

            if (isVisible)
                EventMessenger.Main.Publish(new OnViewShown { View = this });
            else
                EventMessenger.Main.Publish(new OnViewHidden { View = this });
        }
        #endregion

        #region Navigation and Events
        /// <summary>
        /// Bind Events
        /// </summary>
        private void BindEvents() {
            EventMessenger.Main.Subscribe<OnViewNavigated>(OnNavigated);
            EventMessenger.Main.Subscribe<OnViewUpdated>(OnDataUpdated);
        }

        /// <summary>
        /// Unbind Events
        /// </summary>
        private void UnbindEvents() {
            EventMessenger.Main.Unsubscribe<OnViewNavigated>(OnNavigated);
            EventMessenger.Main.Unsubscribe<OnViewUpdated>(OnDataUpdated);
        }

        /// <summary>
        /// On Data Updated
        /// </summary>
        /// <param name="updated"></param>
        public virtual void OnDataUpdated(OnViewUpdated updated) {
            if(updated.View != GetType()) return;
        }

        /// <summary>
        /// On View Navigated
        /// </summary>
        /// <param name="payload"></param>
        public virtual void OnNavigated(OnViewNavigated payload) {
            if(payload.View != GetType()) return;
        }
        #endregion
    }
}