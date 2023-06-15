﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace Input.UIActions {
    public class UIHomeMenuActions : MonoBehaviour {
        [SerializeField] private GameObject homeMenu;
        
        private bool _scrolledUp;
        private bool _scrolledDown;

        private bool _scrolledLeft;
        private bool _scrolledRight;

        private PointerEventData _pointer;

        private float _counter;
        
        private void Start() {
            _pointer = new PointerEventData(EventSystem.current);
            ObjectsReference.Instance.uiHomeMenu.selectedTrigger = homeMenu.GetComponentsInChildren<EventTrigger>()[0];
        }
        
        private void Update() {
            Activate();

            Scroll_Left_Home_Menu_Button();
            Scroll_Right_Home_Menu_Button();
        }

        private void Activate() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) {
                ExecuteEvents.Execute(ObjectsReference.Instance.uiHomeMenu.selectedTrigger.gameObject, _pointer, ExecuteEvents.pointerDownHandler);
            }
        }
        
        private void Scroll_Left_Home_Menu_Button() {
            if (UnityEngine.Input.GetAxis("Horizontal") < 0 && !_scrolledLeft) {
                _scrolledLeft = true;

                ObjectsReference.Instance.uiHomeMenu.SwitchToLeftHomeMenuButton();
            }

            if (UnityEngine.Input.GetAxis("Horizontal") == 0) {
                _scrolledLeft = false;
                _scrolledRight = false;
            }
        }

        private void Scroll_Right_Home_Menu_Button() {
            if (UnityEngine.Input.GetAxis("Horizontal") > 0 && !_scrolledRight) {
                _scrolledRight = true;

                ObjectsReference.Instance.uiHomeMenu.SwitchToRightHomeMenuButton();
            }
            
            if (UnityEngine.Input.GetAxis("Horizontal") == 0) {
                _scrolledLeft = false;
                _scrolledRight = false;
            }
        }
    }
}
