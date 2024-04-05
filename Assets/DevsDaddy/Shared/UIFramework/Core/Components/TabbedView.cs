using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevsDaddy.Shared.UIFramework.Core.Components
{
    [AddComponentMenu("UIFramework/Components/Tabbed View")]
    public sealed class TabbedView : MonoBehaviour
    {
        [Header("Tabbed View")] 
        [SerializeField] private int DefaultTabIndex = 0;
        [SerializeField] private List<TabControl> Tabs = new List<TabControl>();
        
        // Events
        public Action<int> OnTabSwitched;

        private int currentIndex = -1;

        /// <summary>
        /// View Started
        /// </summary>
        private void Start() {
            if (DefaultTabIndex >= 0) {
                SwitchTab(DefaultTabIndex);
            }
            else {
                SwitchTab(-1);
            }
            BindSwitch();
        }

        /// <summary>
        /// View Destroyed
        /// </summary>
        private void OnDestroy() {
            UnbindSwitch();
        }

        /// <summary>
        /// Add Tab Page
        /// </summary>
        /// <param name="tab"></param>
        public void AddTab(TabControl tab) {
            if(Tabs.Contains(tab)) return;
            tab.SwitchButton.onClick.AddListener(() => {
                int newIndex = Tabs.Count;
                SwitchTab(newIndex);
            });
            Tabs.Add(tab);
        }

        /// <summary>
        /// Remove Tab Page
        /// </summary>
        /// <param name="tab"></param>
        public void RemoveTab(TabControl tab) {
            if(!Tabs.Contains(tab)) return;
            tab.SwitchButton.onClick.RemoveAllListeners();
            if(tab.TabPage != null) Destroy(tab.TabPage);
            Tabs.Remove(tab);
        }

        /// <summary>
        /// Remove Tab Page by Index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveTabAt(int index) {
            if (index < Tabs.Count) {
                Tabs[index].SwitchButton.onClick.RemoveAllListeners();
                if(Tabs[index].TabPage != null) Destroy(Tabs[index].TabPage);
                Tabs.RemoveAt(index);
            }
        }

        /// <summary>
        /// Remove All Tabs
        /// </summary>
        public void RemoveAllTabs() {
            UnbindSwitch();
            foreach (var tabControl in Tabs) {
                if(tabControl.TabPage != null)
                    Destroy(tabControl.TabPage);
            }
        }

        /// <summary>
        /// Switch Tab
        /// </summary>
        /// <param name="index"></param>
        public void SwitchTab(int index) {
            if(currentIndex == index) return;
            currentIndex = index;
            for (int i = 0; i < Tabs.Count; i++) {
                Tabs[i].SwitchButton.interactable = (i != currentIndex);
                Tabs[i].TabPage.SetActive((i == currentIndex));
            }
            OnTabSwitched?.Invoke(currentIndex);
        }
        
        /// <summary>
        /// Bind Tab Switch
        /// </summary>
        private void BindSwitch() {
            for (int i = 0; i < Tabs.Count; i++) {
                int index = i;
                Tabs[i].SwitchButton.onClick.AddListener(() => {
                    SwitchTab(index);
                });
            }
        }

        /// <summary>
        /// Unbind Tab Switch
        /// </summary>
        private void UnbindSwitch() {
            foreach (var t in Tabs) {
                t.SwitchButton.onClick.RemoveAllListeners();
            }
        }

        [System.Serializable]
        public class TabControl
        {
            public Button SwitchButton;
            public GameObject TabPage;
        }
    }
}