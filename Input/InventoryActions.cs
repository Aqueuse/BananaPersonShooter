using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryActions : InputActions {
    [SerializeField] private InputActionReference switchToManufacturedItemsInventoryInputActionReference;
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
            
        switchToManufacturedItemsInventoryInputActionReference.action.Enable();
        switchToManufacturedItemsInventoryInputActionReference.action.performed += SwitchToManufacturedItemsInventory;
            
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
        switchToManufacturedItemsInventoryInputActionReference.action.Disable();
        switchToManufacturedItemsInventoryInputActionReference.action.performed -= SwitchToManufacturedItemsInventory;
            
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
        
    private void SwitchToManufacturedItemsInventory(InputAction.CallbackContext callbackContext) {
        ObjectsReference.Instance.uInventoriesManager.Switch_To_Tab(ItemCategory.MANUFACTURED_ITEM);
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