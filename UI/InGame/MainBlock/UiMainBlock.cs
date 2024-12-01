using UI.InGame.Inventory;
using UnityEngine;

namespace UI.InGame.MainBlock {
    public class UiMainBlock : MonoBehaviour {
        [SerializeField] private GenericDictionary<MainBlockType, CanvasGroup> mainCanvasGroupsByBlockTabType;
    
        public void SwitchToBlock(MainBlockType mainBlockType) {
            if (lastFocusedInventoryUITabType == MainBlockType.INVENTORIES)
                ObjectsReference.Instance.uInfobulle.Hide();
            
            foreach (var blockTab in mainCanvasGroupsByBlockTabType) {
                SetActive(blockTab.Value, false);
            }

            SetActive(mainCanvasGroupsByBlockTabType[mainBlockType], true);
            
            if (mainBlockType == MainBlockType.DIALOGUE) {
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GESTION_VIEW);
            }
            
            lastFocusedInventoryUITabType = mainBlockType;
        }
        
        private UInventoriesManager uInventoriesManager;
        private UiMainBlock _uiMainBlock;

        public MainBlockType lastFocusedInventoryUITabType;

        private void Start() {
            uInventoriesManager = ObjectsReference.Instance.uInventoriesManager;
            _uiMainBlock = ObjectsReference.Instance.uiMainBlock;
        }

        public void SwitchToDialogue() { _uiMainBlock.SwitchToBlock(MainBlockType.DIALOGUE); }
        public void SwitchToInventories() { _uiMainBlock.SwitchToBlock(MainBlockType.INVENTORIES); }
        public void SwitchToCommandRoom() { _uiMainBlock.SwitchToBlock(MainBlockType.COMMAND_ROOM); }
        public void SwitchToHelp() { _uiMainBlock.SwitchToBlock(MainBlockType.HELP); }
        
        public void SwitchToLeftTab() {
            switch (lastFocusedInventoryUITabType) {
                case MainBlockType.DIALOGUE:
                    _uiMainBlock.SwitchToBlock(MainBlockType.HELP);
                    break;
                case MainBlockType.HELP:
                    _uiMainBlock.SwitchToBlock(MainBlockType.COMMAND_ROOM);
                    break;
                case MainBlockType.COMMAND_ROOM:
                    _uiMainBlock.SwitchToBlock(MainBlockType.INVENTORIES);
                    uInventoriesManager.SwitchToInventoryTab(DroppedType.BANANA);
                    break;
                case MainBlockType.INVENTORIES:
                    _uiMainBlock.SwitchToBlock(MainBlockType.DIALOGUE);
                    break;
            }
        }

        public void SwitchToRightTab() {
            switch (lastFocusedInventoryUITabType) {
                case MainBlockType.DIALOGUE:
                    _uiMainBlock.SwitchToBlock(MainBlockType.INVENTORIES);
                    uInventoriesManager.SwitchToInventoryTab(DroppedType.BANANA);
                    break;
                case MainBlockType.INVENTORIES:
                    _uiMainBlock.SwitchToBlock(MainBlockType.COMMAND_ROOM);
                    break;
                case MainBlockType.COMMAND_ROOM:
                    _uiMainBlock.SwitchToBlock(MainBlockType.HELP);
                    break;
                case MainBlockType.HELP:
                    _uiMainBlock.SwitchToBlock(MainBlockType.DIALOGUE);
                    break;
            }
        }
    
        public void SetActive(CanvasGroup canvasGroup, bool visible) {
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}
