using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif


namespace InfinityScroll {
    [ExecuteInEditMode]
    public class InfinityScroll : MonoBehaviour {
        [HideInInspector] int _delayEditorUpdate;

        public Canvas canvas;
        public AnimationCurve y;
        [HideInInspector] public float scrollHeight;

        public RectTransform pivot;

//        public IS_Mouse mouse;
        [Tooltip("Position in screen from the scroll")] [Range(-0.001f, 2)]
        public float value;

        [Header("Infinity scrolls settings")] [Tooltip("Distance between  items.")]
        public float marginItems;

        [Tooltip("Start time, when startTime = 0 start to move the scroll.")]
        public float startTime = 2;

        [Tooltip("Constant speed to move the scroll.")]
        public float scrollSpeed = 40;

        public List<IsItem> items;

        void Start() {
            if (!Application.isPlaying) {
                return;
            }

            Setting();
        }
#if UNITY_EDITOR
        private void OnEnable() {
            EditorApplication.update += UpdateManual;
        }
#endif

#if UNITY_EDITOR
        private void OnDisable() {
            EditorApplication.update -= UpdateManual;
        }
#endif

        public void UpdateManual() {
            if (!Application.isPlaying) {
                if (_delayEditorUpdate < 20) {
                    _delayEditorUpdate++;
                }
                else {
                    _delayEditorUpdate = 0;
                    ReloadItems();
                    Setting();
                    UpdateState(value);
                }
            }
        }

        public void Setting() {
            var wrapMode = WrapMode.Loop;
            y.preWrapMode = y.postWrapMode = wrapMode;
        }


        void Update() {
            if (!Application.isPlaying) {
                return;
            }

            var v = 0f;

            if (startTime > 0) startTime -= Time.deltaTime;

            if (startTime <= 0) {
                value += (scrollSpeed / scrollHeight) * Time.deltaTime;
                v = value;
            }
            else {
                v = value;
            }

            UpdateState(v);
        }

        public void UpMouse() {
            value = value + ScrollY();
        }

        public float ScrollY() {
            var hCanvas = (canvas.transform as RectTransform).rect.height;
            var scroll = hCanvas;

            return scroll / (Height());
        }

        public float Height() {
            return scrollHeight + (marginItems * (items.Count - 1));
        }

        public void UpdateState(float v) {
            for (int i = 0; i < items.Count; i++) {
                items[i].Setting();
                items[i].RT().anchoredPosition = new Vector2(0,
                    pivot.anchoredPosition.y + (y.Evaluate(v) * (Height())) - Height());
                v -= ((items[i].Heigh() + ((i == 0) ? 0 : marginItems)) / Height());
            }
        }

        public void ReloadItems() {
            items.Clear();
            scrollHeight = 0;

            for (int i = 0; i < transform.childCount; i++) {
                var item = transform.GetChild(i).GetComponent<IsItem>();

                if (item && item.gameObject.activeSelf) {
                    items.Add(item);
                    scrollHeight += item.Heigh();
                }
            }
        }
    }
}