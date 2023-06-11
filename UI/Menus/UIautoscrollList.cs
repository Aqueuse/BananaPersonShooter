using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus {
    public class UIautoscrollList : MonoBehaviour {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private float scrollOffset;
        
        private int numberOfElementsInScrollList;

        private void Start() {
            numberOfElementsInScrollList = GetComponentsInChildren<HorizontalLayoutGroup>().Length;
        }

        public void SynchronizeScrollbarAndSelectedButton(RectTransform actionButtonRectTransform) {
            var indexInlist = actionButtonRectTransform.GetSiblingIndex();

            scrollRect.verticalNormalizedPosition = 1 - indexInlist  * scrollOffset / numberOfElementsInScrollList;
        }
    }
}
