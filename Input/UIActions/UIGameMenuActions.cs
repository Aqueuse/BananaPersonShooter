﻿using UI;
using UI.Menus;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Input.UIActions {
    public class UIGameMenuActions : MonoBehaviour {
        private bool _scrolledUp;
        private bool _scrolledDown;

        private bool _scrolledLeft;
        private bool _scrolledRight;

        PointerEventData pointer;

        private void Start() {
            pointer = new PointerEventData(EventSystem.current);
        }

        private void Update() {
            Activate();
            Escape();

            Scroll_Left_Game_Menu_Button();
            Scroll_Right_Game_Menu_Button();
        }

        private void Activate() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) {
                ExecuteEvents.Execute(UiGameMenu.Instance.selectedTrigger.gameObject, pointer, ExecuteEvents.pointerDownHandler);
            }
        }

        private void Escape() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                UIManager.Instance.Hide_menus();
            }
        }

        private void Scroll_Left_Game_Menu_Button() {
            if (UnityEngine.Input.GetAxis("Horizontal") < 0 && !_scrolledLeft) {
                _scrolledLeft = true;

                UiGameMenu.Instance.SwitchToLeftButton();
            }

            if (UnityEngine.Input.GetAxis("Horizontal") == 0) {
                _scrolledLeft = false;
                _scrolledRight = false;
            }
        }

        private void Scroll_Right_Game_Menu_Button() {
            if (UnityEngine.Input.GetAxis("Horizontal") > 0 && !_scrolledRight) {
                _scrolledRight = true;
                
                UiGameMenu.Instance.SwitchToRightButton();
            }

            if (UnityEngine.Input.GetAxis("Horizontal") == 0) {
                _scrolledLeft = false;
                _scrolledRight = false;
            }
        }
    }
}