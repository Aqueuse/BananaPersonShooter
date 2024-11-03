using UI.InGame.Inventory;
using UnityEngine;

namespace UI.InGame.MiniChimpBlock {
    public class UiMiniChimpBlock : MonoBehaviour {
        [SerializeField] private GenericDictionary<MiniChimpBlockTabType, CanvasGroup> minichimpCanvasGroupsByBlockTabType;
    
        public void SwitchToBlock(MiniChimpBlockTabType miniChimpBlockTabType) {
            foreach (var blockTab in minichimpCanvasGroupsByBlockTabType) {
                SetActive(blockTab.Value, false);
            }

            SetActive(minichimpCanvasGroupsByBlockTabType[miniChimpBlockTabType], true);
            
            if (miniChimpBlockTabType == MiniChimpBlockTabType.MINICHIMPBLOCK_DIALOGUE) {
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.MINICHIMP_VIEW);
            }
        }
    
        public void SetActive(CanvasGroup canvasGroup, bool visible) {
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}
