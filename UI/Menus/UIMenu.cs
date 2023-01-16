﻿using System;
using UI.InGame.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menus {
    public class UIMenu : MonoSingleton<UIMenu> {
        [SerializeField] private GameObject[] tabs;
        [SerializeField] private Image[] tabsButtons;

        [SerializeField] Color activatedColor;
        [SerializeField] Color unactivatedColor;

        private int selectedTab;

        private void Start() {
            selectedTab = 0;
        }

        public void Switch_to_Tab(int index) {
            for (var i=0; i<tabs.Length; i++) {
                tabs[i].SetActive(false);
                tabsButtons[i].color = unactivatedColor;
            }
        
            tabs[index].SetActive(true);
            tabsButtons[index].color = activatedColor;

            selectedTab = index;
        }

        public void Switch_to_Left_Tab() {
            if (selectedTab == 0) {
                selectedTab = 4;
                Switch_to_Tab(4);
            }

            else {
                selectedTab--;
                Switch_to_Tab(selectedTab);
            }
        }
        
        public void Switch_to_Right_Tab() {
            if (selectedTab == 4) {
                selectedTab = 0;
                Switch_to_Tab(0);
            }

            else {
                selectedTab++;
                Switch_to_Tab(selectedTab);
            }
        }

        public GameObject GetFirstTab() {
            return tabs[0];
        }
    }
}
