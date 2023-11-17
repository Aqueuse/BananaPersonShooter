using Enums;
using Input.interactables;
using UnityEngine;

namespace Input {
    public class InputManager : MonoBehaviour {
        [SerializeField] private GameActions gameActions;
        [SerializeField] private UiActions uiActions;
        [SerializeField] private GestionActions gestionActions;
        [SerializeField] private InventoryActions inventoryActions;
        [SerializeField] private BananaCannonMiniGameActions bananaCannonMiniGameActions;

        public HomeActions homeActions;
        
        public BananasDryerAction bananasDryerAction;
        
        public void SwitchContext(InputContext newInputContext) {
            switch (newInputContext) {
                case InputContext.UI:
                    gameActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = false;
                    gestionActions.enabled = false;
                    inventoryActions.enabled = false;
                    uiActions.enabled = true;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    ObjectsReference.Instance.playerController.canMove = false;
                    break;
                
                case InputContext.GAME:
                    bananaCannonMiniGameActions.enabled = false;
                    gestionActions.enabled = false;
                    inventoryActions.enabled = false;
                    uiActions.enabled = false;
                    gameActions.enabled = true;

                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;

                    ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
                    ObjectsReference.Instance.playerController.canMove = true;
                    break;
                
                case InputContext.GESTION:
                    gameActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = false;
                    inventoryActions.enabled = false;
                    uiActions.enabled = true;
                    gestionActions.enabled = true;
                    
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    ObjectsReference.Instance.playerController.canMove = false;
                    break;
                
                case InputContext.INVENTORY:
                    gameActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = false;
                    gestionActions.enabled = false;
                    uiActions.enabled = true;
                    inventoryActions.enabled = true;
                    
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                
                    ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
                    ObjectsReference.Instance.playerController.canMove = false;
                    break;
                
                case InputContext.BANANA_CANNON_MINI_GAME:
                    gameActions.enabled = false;
                    gestionActions.enabled = false;
                    uiActions.enabled = false;
                    inventoryActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = true;
                    
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    ObjectsReference.Instance.playerController.canMove = false;
                    break;
            }
        }
    }
}
