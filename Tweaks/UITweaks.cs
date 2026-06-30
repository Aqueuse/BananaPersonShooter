using UnityEngine;

namespace Tweaks {
    public static class UITweaks {
        public static void SetCanvasGroupActif(CanvasGroup canvasGroup, bool visible) {
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}