using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityScroll
{

    public class IS_Mouse : MonoBehaviour
    {

        public InfinityScroll infinityScroll;
        [Tooltip("Drag force to move the scroll up or down.")]
        public float dragForce = 1f;


        private bool lastOrientation = false;

        [HideInInspector]
        public bool scrolling = false;
        private Vector2 slider;
        private Vector2 onClickDownPosition;
        private float[] trail;
        private bool multiTouchSwitch = false;
        [HideInInspector]
        public Vector2 offset_mouse;
        private void Start()
        {
            trail = new float[2];
            //while the roulette is active, deactivates the multitouch
            if (Input.multiTouchEnabled)
            {
                Input.multiTouchEnabled = false;
                multiTouchSwitch = true;
            }
        }

        public bool IsScrolling() { return scrolling; }
        public bool IsLastOrientation() { return lastOrientation; }

        private void StartScroller()
        {
            onClickDownPosition = Input.mousePosition;
            trail[0] = trail[1] = GetMousePositionY();

            scrolling = true;
        }

        public void EndScroller()
        {
            infinityScroll.UpMouse();
            lastOrientation = trail[1] < GetMousePositionY();
            scrolling = false;
            slider = Vector2.zero;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) { StartScroller(); }
            if (Input.GetMouseButtonUp(0)) { EndScroller(); }

            if (scrolling)
            {
                slider.x = (Input.mousePosition.x - onClickDownPosition.x) / Screen.height;
                slider.y = (Input.mousePosition.y - onClickDownPosition.y) / Screen.height;
                offset_mouse = slider * dragForce * (1f);

                saveTrial();
            }
        }

        void saveTrial()
        {
            if (trail[0] != GetMousePositionY())
            {
                trail[1] = trail[0];
                trail[0] = GetMousePositionY();
            }
        }

        public void ResetOffset() { offset_mouse = Vector2.zero; }
        public float GetOffsetX() { return offset_mouse.x; }
        public float GetOffsetY() { return offset_mouse.y; }

        public float GetMousePositionY() { return Input.mousePosition.y; }
        public float GetMousePositionX() { return Input.mousePosition.x; }

        private void OnDestroy()
        {
            if (multiTouchSwitch)
                Input.multiTouchEnabled = true;
        }

    }
}