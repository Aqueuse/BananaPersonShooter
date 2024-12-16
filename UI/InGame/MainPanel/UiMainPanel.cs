using UI.InGame.MainPanel.Inventories;
using UnityEngine;

namespace UI.InGame.MainPanel {
    public class UiMainPanel : MonoBehaviour {
        [SerializeField] private GenericDictionary<MainBlockType, CanvasGroup> mainCanvasGroupsByBlockTabType;
    
        private UInventoriesManager uInventoriesManager;

        public MainBlockType lastFocusedBlock = MainBlockType.DIALOGUE;

        private void Start() {
            uInventoriesManager = ObjectsReference.Instance.uInventoriesManager;
        }

        public void SwitchToDialogue() { SwitchToBlock(MainBlockType.DIALOGUE); }
        public void SwitchToInventories() { SwitchToBlock(MainBlockType.INVENTORIES); }
        public void SwitchToCommandRoom() { SwitchToBlock(MainBlockType.COMMAND_ROOM); }
        public void SwitchToHelp() { SwitchToBlock(MainBlockType.HELP); }

        private void SwitchToBlock(MainBlockType mainBlockType) {
            if (lastFocusedBlock == MainBlockType.INVENTORIES)
                ObjectsReference.Instance.uInfobulle.Hide();
            
            foreach (var blockTab in mainCanvasGroupsByBlockTabType) {
                SetActive(blockTab.Value, false);
            }

            SetActive(mainCanvasGroupsByBlockTabType[mainBlockType], true);

            if (mainBlockType == MainBlockType.INVENTORIES) {
                uInventoriesManager.SwitchToLastFocusedInventory();
            }
            
            lastFocusedBlock = mainBlockType;
        }
        
        public void SwitchToLeftTab() {
            switch (lastFocusedBlock) {
                case MainBlockType.DIALOGUE:
                    SwitchToBlock(MainBlockType.HELP);
                    break;
                case MainBlockType.HELP:
                    SwitchToBlock(MainBlockType.COMMAND_ROOM);
                    break;
                case MainBlockType.COMMAND_ROOM:
                    SwitchToBlock(MainBlockType.INVENTORIES);
                    uInventoriesManager.SwitchToInventoryTab(DroppedType.BANANA);
                    break;
                case MainBlockType.INVENTORIES:
                    SwitchToBlock(MainBlockType.DIALOGUE);
                    break;
            }
        }

        public void SwitchToRightTab() {
            switch (lastFocusedBlock) {
                case MainBlockType.DIALOGUE:
                    SwitchToBlock(MainBlockType.INVENTORIES);
                    uInventoriesManager.SwitchToInventoryTab(DroppedType.BANANA);
                    break;
                case MainBlockType.INVENTORIES:
                    SwitchToBlock(MainBlockType.COMMAND_ROOM);
                    break;
                case MainBlockType.COMMAND_ROOM:
                    SwitchToBlock(MainBlockType.HELP);
                    break;
                case MainBlockType.HELP:
                    SwitchToBlock(MainBlockType.DIALOGUE);
                    break;
            }
        }

        public void SwitchToLastFocusedBlock() {
            SwitchToBlock(lastFocusedBlock);
        }
    
        public void SetActive(CanvasGroup canvasGroup, bool visible) {
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}
