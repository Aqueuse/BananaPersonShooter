using UI.InGame.Inventory;
using UI.InGame.MiniChimpBlock;
using UnityEngine;

namespace UI.InGame {
    public class UIBananaGun : MonoBehaviour {
        private UInventoriesManager uInventoriesManager;
        private UiMiniChimpBlock uiMiniChimpBlock;

        public BananaGunUITabType lastFocusedBananaGunUITabType;

        private void Start() {
            uInventoriesManager = ObjectsReference.Instance.uInventoriesManager;
            uiMiniChimpBlock = ObjectsReference.Instance.uiMiniChimpBlock;
        }

        public void SwitchToDroppedInventory() {
            uInventoriesManager.SwitchToInventoryTab(ItemCategory.DROPPED);
            lastFocusedBananaGunUITabType = BananaGunUITabType.INVENTORY_DROPPED;
            
            ObjectsReference.Instance.uInfobulle.Hide();
        }

        public void SwitchToIngredientsInventory() {
            uInventoriesManager.SwitchToInventoryTab(ItemCategory.INGREDIENT);
            lastFocusedBananaGunUITabType = BananaGunUITabType.INVENTORY_INGREDIENTS;
            
            ObjectsReference.Instance.uInfobulle.Hide();
        }

        public void SwitchToManufacturedItemsInventory() {
            uInventoriesManager.SwitchToInventoryTab(ItemCategory.MANUFACTURED_ITEM);
            lastFocusedBananaGunUITabType = BananaGunUITabType.INVENTORY_MANUFACTURED_ITEMS;
            
            ObjectsReference.Instance.uInfobulle.Hide();
        }

        public void SwitchToBuildablesInventory() {
            uInventoriesManager.SwitchToInventoryTab(ItemCategory.BUILDABLE);
            lastFocusedBananaGunUITabType = BananaGunUITabType.INVENTORY_BUILDABLES;
            
            ObjectsReference.Instance.uInfobulle.Hide();
        }

        public void SwitchToMiniChimpDialogue() {
            uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_DIALOGUE);
            lastFocusedBananaGunUITabType = BananaGunUITabType.MINICHIMPBLOCK_DIALOGUE;
            
            ObjectsReference.Instance.uInfobulle.Hide();
        }

        public void SwitchToDescription() {
            uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_DESCRIPTION);
            lastFocusedBananaGunUITabType = BananaGunUITabType.MINICHIMPBLOCK_DESCRIPTION;
        }

        public void SwitchToMap() {
            uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_MAP);
            lastFocusedBananaGunUITabType = BananaGunUITabType.MINICHIMPBLOCK_MAP;
        }

        public void SwitchToBananapedia() {
            uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_BANANAPEDIA);
            lastFocusedBananaGunUITabType = BananaGunUITabType.MINICHIMPBLOCK_BANANAPEDIA;
        }

        public void SwitchToHelp() {
            uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_HELP);
            lastFocusedBananaGunUITabType = BananaGunUITabType.MINICHIMPBLOCK_HELP;
        }
        
        public void SwitchToLeftTab() {
            switch (lastFocusedBananaGunUITabType) {
                case BananaGunUITabType.MINICHIMPBLOCK_HELP:
                    uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_BANANAPEDIA);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.MINICHIMPBLOCK_BANANAPEDIA;
                    break;
                case BananaGunUITabType.MINICHIMPBLOCK_BANANAPEDIA:
                    uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_MAP);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.MINICHIMPBLOCK_MAP;
                    break;
                case BananaGunUITabType.MINICHIMPBLOCK_MAP:
                    uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_DESCRIPTION);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.MINICHIMPBLOCK_DESCRIPTION;
                    break;
                case BananaGunUITabType.MINICHIMPBLOCK_DESCRIPTION:
                    uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_DIALOGUE);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.MINICHIMPBLOCK_DIALOGUE;
                    break;
                case BananaGunUITabType.MINICHIMPBLOCK_DIALOGUE:
                    uInventoriesManager.SwitchToInventoryTab(ItemCategory.BUILDABLE);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.INVENTORY_BUILDABLES;
                    break;
                case BananaGunUITabType.INVENTORY_BUILDABLES:
                    uInventoriesManager.SwitchToInventoryTab(ItemCategory.MANUFACTURED_ITEM);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.INVENTORY_MANUFACTURED_ITEMS;
                    break;
                case BananaGunUITabType.INVENTORY_MANUFACTURED_ITEMS:
                    uInventoriesManager.SwitchToInventoryTab(ItemCategory.INGREDIENT);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.INVENTORY_INGREDIENTS;
                    break;
                case BananaGunUITabType.INVENTORY_INGREDIENTS:
                    uInventoriesManager.SwitchToInventoryTab(ItemCategory.DROPPED);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.INVENTORY_DROPPED;
                    break;
                case BananaGunUITabType.INVENTORY_DROPPED:
                    uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_HELP);
                    break;
            }
        }

        public void SwitchToRightTab() {
            switch (lastFocusedBananaGunUITabType) {
                case BananaGunUITabType.INVENTORY_DROPPED:
                    uInventoriesManager.SwitchToInventoryTab(ItemCategory.INGREDIENT);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.INVENTORY_INGREDIENTS;
                    break;
                case BananaGunUITabType.INVENTORY_INGREDIENTS:
                    uInventoriesManager.SwitchToInventoryTab(ItemCategory.MANUFACTURED_ITEM);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.INVENTORY_MANUFACTURED_ITEMS;
                    break;
                case BananaGunUITabType.INVENTORY_MANUFACTURED_ITEMS:
                    uInventoriesManager.SwitchToInventoryTab(ItemCategory.BUILDABLE);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.INVENTORY_BUILDABLES;
                    break;
                case BananaGunUITabType.INVENTORY_BUILDABLES:
                    uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_DIALOGUE);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.MINICHIMPBLOCK_DIALOGUE;
                    break;
                case BananaGunUITabType.MINICHIMPBLOCK_DIALOGUE:
                    uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_DESCRIPTION);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.MINICHIMPBLOCK_DESCRIPTION;
                    break;
                case BananaGunUITabType.MINICHIMPBLOCK_DESCRIPTION:
                    uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_MAP);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.MINICHIMPBLOCK_MAP;
                    break;
                case BananaGunUITabType.MINICHIMPBLOCK_MAP:
                    uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_BANANAPEDIA);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.MINICHIMPBLOCK_BANANAPEDIA;
                    break;
                case BananaGunUITabType.MINICHIMPBLOCK_BANANAPEDIA:
                    uiMiniChimpBlock.SwitchToBlock(MiniChimpBlockTabType.MINICHIMPBLOCK_HELP);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.MINICHIMPBLOCK_HELP;
                    break;
                case BananaGunUITabType.MINICHIMPBLOCK_HELP:
                    uInventoriesManager.SwitchToInventoryTab(ItemCategory.DROPPED);
                    lastFocusedBananaGunUITabType = BananaGunUITabType.INVENTORY_DROPPED;
                    break;
            }
        }
    }
}
