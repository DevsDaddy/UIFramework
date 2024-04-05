using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DevsDaddy.Shared.UIFramework.Core.ShaderEffect.Effects
{
    [AddComponentMenu("UIFramework/Effects/Ripple Effect")]
    [DisallowMultipleComponent]
    public class UIRippleEffect : MonoBehaviour, IPointerClickHandler
    {
        [Header("Ripple Setup")]
        [Tooltip("Ripple Effect Sprite")]
        [SerializeField] Sprite m_EffectSprite;
        
        [Tooltip("Ripple Color")]
        [SerializeField] private Color RippleColor;
        
        [Tooltip("Ripple Max Power")]
        [SerializeField] private float MaxPower = .25f;
        
        [Tooltip("Ripple Effect Duration")]
        [SerializeField] private float Duration = .25f;
        
        private static Sprite _defaultEffectSprite;
        
        public Sprite effectSprite
        {
            get
            {
                return m_EffectSprite
                    ? m_EffectSprite
                    : defaultEffectSprite;
            }
            set
            {
                if (m_EffectSprite != null && m_EffectSprite == value) return;
                m_EffectSprite = value;
            }
        }

        private static Sprite defaultEffectSprite
        {
            get
            {
                return _defaultEffectSprite
                    ? _defaultEffectSprite
                    : (_defaultEffectSprite = Resources.Load<Sprite>("Default-Ripple"));
            }
        }
        
        private bool m_IsInitialized = false;
        private RectMask2D m_RectMask;

        /// <summary>
        /// Setup Ripple
        /// </summary>
        private void Awake() {
            if (effectSprite == null) {
                Debug.LogWarning("Failed to add ripple graphics. Not Ripple found.");
                return;
            }

            SetupRippleContainer();
        }

        /// <summary>
        /// Setup Ripple Container
        /// </summary>
        private void SetupRippleContainer() {
            m_RectMask = gameObject.AddComponent<RectMask2D>();
            m_RectMask.padding = new Vector4(5, 5, 5, 5);
            m_RectMask.softness = new Vector2Int(20, 20);
            m_IsInitialized = true;
        }
        
        /// <summary>
        /// Get Click Data
        /// </summary>
        /// <param name="pointerEventData"></param>
        public void OnPointerClick(PointerEventData pointerEventData) {
            if(!m_IsInitialized) return;
            GameObject rippleObject = new GameObject("_ripple_");
            LayoutElement crl = rippleObject.AddComponent<LayoutElement>();
            crl.ignoreLayout = true;
            
            Image currentRippleImage = rippleObject.AddComponent<Image>();
            currentRippleImage.sprite = effectSprite;
            currentRippleImage.transform.SetAsLastSibling();
            currentRippleImage.transform.SetPositionAndRotation(pointerEventData.position, Quaternion.identity);
            currentRippleImage.transform.SetParent(transform);
            currentRippleImage.color = new Color(RippleColor.r, RippleColor.g, RippleColor.b, 0f);
            currentRippleImage.raycastTarget = false;
            StartCoroutine(AnimateRipple(rippleObject.GetComponent<RectTransform>(), currentRippleImage, () => {
                currentRippleImage = null;
                Destroy(rippleObject);
                StopCoroutine(nameof(AnimateRipple));
            }));
        }

        /// <summary>
        /// Animate Ripple
        /// </summary>
        /// <param name="rippleTransform"></param>
        /// <param name="rippleImage"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        private IEnumerator AnimateRipple(RectTransform rippleTransform, Image rippleImage, Action onComplete) {
            Vector2 initialSize = Vector2.zero;
            Vector2 targetSize = new Vector2(150,150);
            Color initialColor = new Color(RippleColor.r, RippleColor.g, RippleColor.b, MaxPower);
            Color targetColor = new Color(RippleColor.r, RippleColor.g, RippleColor.b, 0f);
            float elapsedTime = 0f;
            
            while (elapsedTime < Duration)
            {
                elapsedTime += Time.deltaTime;
                rippleTransform.sizeDelta = Vector2.Lerp(initialSize, targetSize, elapsedTime / Duration);
                rippleImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / Duration);
                yield return null;
            }
            
            onComplete?.Invoke();
        }
    }
}