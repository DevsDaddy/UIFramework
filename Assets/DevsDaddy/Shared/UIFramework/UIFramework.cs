using System;
using System.Collections.Generic;
using DevsDaddy.Shared.EventFramework;
using DevsDaddy.Shared.UIFramework.Core;
using DevsDaddy.Shared.UIFramework.Core.Constants;
using DevsDaddy.Shared.UIFramework.Core.Wrapper;
using DevsDaddy.Shared.UIFramework.Payloads;
using UnityEngine;

namespace DevsDaddy.Shared.UIFramework
{
    /// <summary>
    /// UI Framework Container
    /// </summary>
    public static class UIFramework
    {
        // Current Views
        private static readonly List<IBaseView> currentViews = new List<IBaseView>();
        private static CoroutineWrapper currentWrapper;
        
        // Navigation
        private static bool systemEventBinded = false;
        private static readonly List<IBaseView> viewHistory = new List<IBaseView>();
        private static IBaseView homeView = null;
        private static IBaseView currentView = null;

        /// <summary>
        /// Load View from Resources
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <param name="isHomeView"></param>
        /// <param name="onComplete"></param>
        /// <param name="onError"></param>
        /// <typeparam name="T"></typeparam>
        public static void LoadViewFromResources<T>(string resourcePath, bool isHomeView = false, Action<T> onComplete = null, Action<string> onError = null) where T : class, IBaseView {
            // Load Object
            GameObject loadedObject = Resources.Load<GameObject>(resourcePath);
            if (loadedObject == null) {
                onError?.Invoke($"Failed to load View from Resources: {resourcePath}");
                Debug.LogWarning($"Failed to load View from Resources: {resourcePath}");
                return;
            }

            // Get View Reference
            T viewReference = loadedObject.GetComponent<T>();
            if (viewReference == null) {
                onError?.Invoke($"Game object at resource {resourcePath} is not contain view of type {typeof(T)}");
                Debug.LogWarning($"Game object at resource {resourcePath} is not contain view of type {typeof(T)}");
                return;
            }

            // Bind View
            onComplete?.Invoke((T)BindView<T>(viewReference, isHomeView));
        }

        /// <summary>
        /// Get View by Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetView<T>() where T : class, IBaseView {
            var found = currentViews.Find(view => view != null && view.GetType() == typeof(T));
            if(found ==  null) Debug.LogWarning($"Failed to find view of type {typeof(T)}");
            return (T)found;
        }

        /// <summary>
        /// Bind / Rebind View
        /// </summary>
        /// <param name="view"></param>
        /// <param name="isHomeView"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IBaseView BindView<T>(T view, bool isHomeView = false) where T : class, IBaseView {
            // Bind System Events
            if (!systemEventBinded)
                EventMessenger.Main.Subscribe<OnViewHomeRequested>(OnHomeRequested);
            
            // Found Exists View
            if (view == null) {
                Debug.LogError("Failed to bind view. View references is null");
                return null;
            }
            var found = currentViews.Find(viewFound => viewFound != null && viewFound.GetType() == typeof(T));
            if (found != null) {
                EventMessenger.Main.Publish(new OnViewRemoved { View = found });
                currentViews.Remove(found);
                currentViews.Add(view);
                EventMessenger.Main.Publish(new OnViewAdded { View = view });
                if (isHomeView) {
                    homeView = view;
                    Navigate(view);
                }
                return view;
            }
            
            currentViews.Add(view);
            EventMessenger.Main.Publish(new OnViewAdded { View = view });
            if (isHomeView) {
                homeView = view;
                Navigate(view);
            }
            return view;
        }

        /// <summary>
        /// Unbind View by Reference / Type
        /// </summary>
        /// <param name="view"></param>
        /// <typeparam name="T"></typeparam>
        public static void UnbindView<T>(T view = null) where T : class, IBaseView {
            // Find View by Reference
            if (view != null && currentViews.Contains(view)) {
                EventMessenger.Main.Publish(new OnViewRemoved { View = view });
                currentViews.Remove(view);
                return;
            }
            
            // Find by Type
            var found = currentViews.Find(viewFound => viewFound != null && viewFound.GetType() == typeof(T));
            if (found != null) {
                EventMessenger.Main.Publish(new OnViewRemoved { View = found });
                currentViews.Remove(found);
            }
        }

        /// <summary>
        /// Navigate to View
        /// </summary>
        /// <param name="view"></param>
        /// <param name="options"></param>
        /// <param name="pushInHistory"></param>
        public static void Navigate(IBaseView view, DisplayOptions options = null) {
            if(currentView == view) return;
            
            // Hide Current and Show New
            if(currentView != null)
                currentView.HideView(options);
            
            currentView = view;
            currentView.ShowView(options);
            
            // Push in History
            viewHistory.Add(view);
        }

        /// <summary>
        /// Get Current View
        /// </summary>
        public static IBaseView GetCurrentView() {
            return currentView;
        }

        /// <summary>
        /// Close Curren View and Go Back
        /// </summary>
        public static void GoBack(DisplayOptions options = null) {
            Debug.Log(viewHistory.Count);
            Debug.Log(viewHistory[0].GetType());
            if (viewHistory.Count < 1 || currentView == null) return;
            if(viewHistory.Count == 1 && viewHistory[0] == homeView && currentView == homeView) return;
            currentView.HideView(options);
            
            int lastView = viewHistory.Count - 1;
            if (viewHistory[lastView].GetType() == currentView.GetType()) {
                viewHistory.RemoveAt(lastView);
                currentView = viewHistory[lastView-1];
                currentView.ShowView(options);
            }
        }

        /// <summary>
        /// Go Home
        /// </summary>
        /// <param name="closeAll"></param>
        public static void GoHome(bool closeAll = false) {
            if(homeView == null) return;
            if (!closeAll) {
                if(currentView == homeView) return;
                Navigate(homeView);
                return;
            }
            
            // Close All
            viewHistory.Clear();
            Navigate(homeView);
        }

        /// <summary>
        /// Get Current Mono Wrapper
        /// </summary>
        /// <returns></returns>
        public static CoroutineWrapper GetWrapper(){
            if (currentWrapper == null) {
                GameObject obj = new GameObject(GeneralConstants.ROUTINE_WRAPPER_NAME);
                obj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                currentWrapper = obj.AddComponent<CoroutineWrapper>();
            }

            return currentWrapper;
        }

        /// <summary>
        /// On View Home Requested
        /// </summary>
        /// <param name="requested"></param>
        private static void OnHomeRequested(OnViewHomeRequested requested) {
            GoHome(requested.CloseAll);
        }
    }
}