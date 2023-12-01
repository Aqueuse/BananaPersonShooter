using Interactions.InteractionsActions;
using UnityEngine;

namespace Input {
    public class InputManager : MonoBehaviour {
        [SerializeField] private GameActions gameActions;
        [SerializeField] private UiActions uiActions;
        [SerializeField] private GestionActions gestionActions;
        [SerializeField] private InventoryActions inventoryActions;
        [SerializeField] private BananaCannonMiniGameActions bananaCannonMiniGameActions;

        public HomeActions homeActions;
        
        public BananasDryerAddPeelInteraction bananasDryerAddPeelInteraction;
        
        public void SwitchContext(InputContext newInputContext) {
            switch (newInputContext) {
                case InputContext.UI:
                    homeActions.enabled = false;
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
                    homeActions.enabled = false;

                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;

                    ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
                    
                    ObjectsReference.Instance.playerController.canMove = true;
                    ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = false;
                    break;
                
                case InputContext.GESTION:
                    gameActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = false;
                    inventoryActions.enabled = false;
                    uiActions.enabled = true;
                    gestionActions.enabled = true;
                    homeActions.enabled = false;
                    
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    ObjectsReference.Instance.playerController.canMove = false;
                    ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
                    break;
                
                case InputContext.INVENTORY:
                    gameActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = false;
                    gestionActions.enabled = false;
                    uiActions.enabled = true;
                    inventoryActions.enabled = true;
                    homeActions.enabled = false;
                    
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                
                    ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
                    ObjectsReference.Instance.playerController.canMove = false;
                    ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
                    break;
                
                case InputContext.BANANA_CANNON_MINI_GAME:
                    gameActions.enabled = false;
                    gestionActions.enabled = false;
                    uiActions.enabled = true;
                    inventoryActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = true;
                    homeActions.enabled = false;
                    
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    ObjectsReference.Instance.playerController.canMove = false;
                    ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
                    break;
                case InputContext.HOME:
                    gameActions.enabled = false;
                    gestionActions.enabled = false;
                    uiActions.enabled = true;
                    inventoryActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = false;
                    homeActions.enabled = true;
                    
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    ObjectsReference.Instance.playerController.canMove = false;
                    ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
                    break;
            }
        }
    }
}
