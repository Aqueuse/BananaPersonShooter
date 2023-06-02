using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UI {
    [ExecuteInEditMode]
    public class InfinityScroll : MonoBehaviour {
        int delayEditorUpdate;

        public AnimationCurve y;
        [HideInInspector] public float scrollHeight;

        public RectTransform pivot;

        [Tooltip("Position in screen from the scroll")] [Range(-0.001f, 2)]
        public float value;

        [Header("Infinity scrolls settings")] [Tooltip("Distance between  items.")]
        public float marginItems;

        [Tooltip("Start time, when startTime = 0 start to move the scroll.")]
        public float startTime = 2;
        
        public List<IS_Item> items;

        private float verticalValue;
        
        private void Start() {
            if (!Application.isPlaying) {
                return;
            }

            Setting();
        }
        
        private void OnEnable() {
            EditorApplication.update += UpdateManual;
        }

        private void OnDisable() {
            EditorApplication.update -= UpdateManual;
        }

        private void UpdateManual() {
            if (!Application.isPlaying) {
                if (delayEditorUpdate < 20) {
                    delayEditorUpdate++;
                }
                else {
                    delayEditorUpdate = 0;
                    ReloadItems();
                    Setting();
                    UpdateState(value);
                }
            }
        }

        private void Setting() {
            var wrap_mode = WrapMode.Loop;
            y.preWrapMode = y.postWrapMode = wrap_mode;
        }


        private void Update() {
            if (!Application.isPlaying) {
                return;
            }
            
            if (startTime > 0) startTime -= Time.deltaTime;

            verticalValue = value;

            UpdateState(verticalValue);
        }

        private float Height() {
            return scrollHeight + (marginItems * (items.Count - 1));
        }

        private void UpdateState(float v) {
            for (var i = 0; i < items.Count; i++) {
                items[i].Setting();
                items[i].RT().anchoredPosition = new Vector2(0,
                    pivot.anchoredPosition.y + y.Evaluate(v) * Height() - Height());
                v -= (items[i].Heigh() + (i == 0 ? 0 : marginItems)) / Height();
            }
        }

        public void ReloadItems() {
            items.Clear();
            scrollHeight = 0;

            for (var i = 0; i < transform.childCount; i++) {
                var item = transform.GetChild(i).GetComponent<IS_Item>();

                if (item && item.gameObject.activeSelf) {
                    items.Add(item);
                    scrollHeight += item.Heigh();
                }
            }
        }
    }
}