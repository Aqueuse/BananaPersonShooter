using InGame.Items.ItemsProperties.Bananas;
using UI.InGame.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    public class InventoryActions : InputActions {
        [SerializeField] private InputActionReference selectBananaInputActionReference;
        
        [SerializeField] private InputActionReference switchToBananaInventoryInputActionReference;
        [SerializeField] private InputActionReference switchToRawMaterialInventoryInputActionReference;
        [SerializeField] private InputActionReference switchToIngredientsInventoryInputActionReference;
        
        [SerializeField] private InputActionReference switchToLeftInventoryInputActionReference;
        [SerializeField] private InputActionReference switchToRightInventoryInputActionReference;

        [SerializeField] private InputActionReference switchBackToGameInputActionReference;
        
        private void OnEnable() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            ObjectsReference.Instance.uiActions.enabled = true;

            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;

            selectBananaInputActionReference.action.Enable();
            selectBananaInputActionReference.action.performed += SelectBanana;

            switchToBananaInventoryInputActionReference.action.Enable();
            switchToBananaInventoryInputActionReference.action.performed += SwitchToBananasInventory;
            
            switchToRawMaterialInventoryInputActionReference.action.Enable();
            switchToRawMaterialInventoryInputActionReference.action.performed += SwitchToRawMaterialsInventory;
            
            switchToIngredientsInventoryInputActionReference.action.Enable();
            switchToIngredientsInventoryInputActionReference.action.performed += SwitchToIngredientsInventory;
            
            switchToLeftInventoryInputActionReference.action.Enable();
            switchToLeftInventoryInputActionReference.action.performed += SwitchToLeft;

            switchToRightInventoryInputActionReference.action.Enable();
            switchToRightInventoryInputActionReference.action.performed += SwitchToRight;
            
            switchBackToGameInputActionReference.action.Enable();
            switchBackToGameInputActionReference.action.performed += SwitchBackToGame;
        }
        
        private void OnDisable() {
            selectBananaInputActionReference.action.Disable();
            selectBananaInputActionReference.action.performed -= SelectBanana;
            
            switchToBananaInventoryInputActionReference.action.Disable();
            switchToBananaInventoryInputActionReference.action.performed -= SwitchToBananasInventory;
            
            switchToRawMaterialInventoryInputActionReference.action.Disable();
            switchToRawMaterialInventoryInputActionReference.action.performed -= SwitchToRawMaterialsInventory;
            
            switchToIngredientsInventoryInputActionReference.action.Disable();
            switchToIngredientsInventoryInputActionReference.action.performed -= SwitchToIngredientsInventory;
            
            switchToLeftInventoryInputActionReference.action.Disable();
            switchToLeftInventoryInputActionReference.action.performed -= SwitchToLeft;

            switchToRightInventoryInputActionReference.action.Disable();
            switchToRightInventoryInputActionReference.action.performed -= SwitchToRight;
            
            switchBackToGameInputActionReference.action.Disable();
            switchBackToGameInputActionReference.action.performed -= SwitchBackToGame;
        }

        private void SelectBanana(InputAction.CallbackContext callbackContext) {
            var lastSelectedBananaItemScriptableObject = ObjectsReference.Instance.uInventoriesManager.lastSelectedItemByInventoryCategory[ItemCategory.BANANA].GetComponent<UInventorySlot>().itemScriptableObject;

            if (lastSelectedBananaItemScriptableObject.bananaType != BananaType.EMPTY) { ObjectsReference.Instance.bananaMan.activeItem = (BananasPropertiesScriptableObject)lastSelectedBananaItemScriptableObject;
            }
        }
        
        private void SwitchToBananasInventory(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.uInventoriesManager.Switch_To_Tab(ItemCategory.BANANA);
        }
        
        private void SwitchToRawMaterialsInventory(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.uInventoriesManager.Switch_To_Tab(ItemCategory.RAW_MATERIAL);
        }

        private void SwitchToIngredientsInventory(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.uInventoriesManager.Switch_To_Tab(ItemCategory.INGREDIENT);
        }
        
        private static void SwitchToLeft(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.uInventoriesManager.Switch_To_Left_Tab();
        }

        private static void SwitchToRight(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.uInventoriesManager.Switch_To_Right_Tab();
        }

        private void SwitchBackToGame(InputAction.CallbackContext context) {
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.INVENTORIES, false);
                ObjectsReference.Instance.descriptionsManager.HideAllPanels();
                
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
                ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
        }
    }
}
