using System;
using System.Collections;
using DevsDaddy.Shared.UIFramework.Core.Extensions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DevsDaddy.Shared.UIFramework.Core.Components
{
    [AddComponentMenu("UIFramework/Components/Web Image")]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class WebImage : MonoBehaviour
    {
        [Header("Web Image Parameters")] 
        [SerializeField] private string spriteUrl = "";
        [SerializeField] private bool loadAtVisible = false;
        [SerializeField] private bool loadAtStart = false;

        // Image Container
        private RectTransform m_CurrentRect;
        private Image m_ImageContainer;

        // Is Visible at Camera
        private bool m_IsVisible = false;

        // Loading Data
        private bool m_IsLoading = false;
        private string m_CurrentCoroutineId;

        /// <summary>
        /// On Image Awake
        /// </summary>
        private void Awake() {
            m_ImageContainer = GetComponent<Image>();
            m_CurrentRect = GetComponent<RectTransform>();
            m_IsVisible = m_CurrentRect.IsVisibleFrom();
        }

        /// <summary>
        /// Load Image
        /// </summary>
        private void Start() {
            if(loadAtStart && !m_IsLoading)
                LoadImage();
        }

        /// <summary>
        /// On Destroy
        /// </summary>
        private void OnDestroy() {
            if(!string.IsNullOrEmpty(m_CurrentCoroutineId))
                UIFramework.GetWrapper().StopRoutine(m_CurrentCoroutineId);
        }

        /// <summary>
        /// On Update
        /// </summary>
        private void Update() {
            if(m_CurrentRect == null || m_ImageContainer == null) return;
            m_IsVisible = m_CurrentRect.IsVisibleFrom();
            if (loadAtVisible && !m_IsLoading) {
                LoadImage();
            }
        }

        /// <summary>
        /// Load Image
        /// </summary>
        private void LoadImage() {
            if(m_IsLoading) return;
            m_IsLoading = true;
            m_CurrentCoroutineId = "WebImageRequest"+gameObject.GetInstanceID();
            UIFramework.GetWrapper().StartRoutine(m_CurrentCoroutineId, LoadSprite(sprite => {
                m_ImageContainer.sprite = sprite;
                m_IsLoading = false;
                UIFramework.GetWrapper().StopRoutine(m_CurrentCoroutineId);
                m_CurrentCoroutineId = null;
            }, error => throw new Exception(error)));
        }

        /// <summary>
        /// Load Sprite
        /// </summary>
        /// <param name="onComplete"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        private IEnumerator LoadSprite(Action<Sprite> onComplete, Action<string> onError = null) {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(spriteUrl);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                onError?.Invoke("Failed to download texture. Error: " + request.error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(request);
                Sprite result = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                onComplete?.Invoke(result);
            }
        }
    }
}