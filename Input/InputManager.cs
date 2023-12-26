using UnityEngine;

namespace Input {
    public class InputManager : MonoBehaviour {
        [SerializeField] private GameActions gameActions;
        [SerializeField] private HomeActions homeActions;
        [SerializeField] private UiActions uiActions;
        [SerializeField] private GestionActions gestionActions;
        [SerializeField] private InventoryActions inventoryActions;
        [SerializeField] private BananaCannonMiniGameActions bananaCannonMiniGameActions;
        [SerializeField] private VisitorsWaitingListActions visitorsWaitingListActions;
        [SerializeField] private GestionCommandRoomPanelActions gestionCommandRoomPanelActions;
        
        public void SwitchContext(InputContext newInputContext) {
            switch (newInputContext) {
                case InputContext.UI:
                    homeActions.enabled = false;
                    gameActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = false;
                    visitorsWaitingListActions.enabled = false;
                    gestionActions.enabled = false;
                    inventoryActions.enabled = false;
                    gestionCommandRoomPanelActions.enabled = false;
                    uiActions.enabled = true;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    ObjectsReference.Instance.playerController.canMove = false;
                    break;
                
                case InputContext.GAME:
                    bananaCannonMiniGameActions.enabled = false;
                    visitorsWaitingListActions.enabled = false;
                    gestionActions.enabled = false;
                    inventoryActions.enabled = false;
                    uiActions.enabled = false;
                    homeActions.enabled = false;
                    gestionCommandRoomPanelActions.enabled = false;
                    gameActions.enabled = true;
                    
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;

                    ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
                    
                    ObjectsReference.Instance.playerController.canMove = true;
                    ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = false;
                    break;
                
                case InputContext.GESTION:
                    gameActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = false;
                    visitorsWaitingListActions.enabled = false;
                    inventoryActions.enabled = false;
                    homeActions.enabled = false;
                    gestionCommandRoomPanelActions.enabled = false;
                    gestionActions.enabled = true;
                    uiActions.enabled = true;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    ObjectsReference.Instance.playerController.canMove = false;
                    ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
                    break;
                
                case InputContext.INVENTORY:
                    gameActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = false;
                    visitorsWaitingListActions.enabled = false;
                    gestionActions.enabled = false;
                    homeActions.enabled = false;
                    gestionCommandRoomPanelActions.enabled = false;
                    inventoryActions.enabled = true;
                    uiActions.enabled = true;
                    
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                
                    ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
                    ObjectsReference.Instance.playerController.canMove = false;
                    ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
                    break;
                
                case InputContext.BANANA_CANNON_MINI_GAME:
                    gameActions.enabled = false;
                    gestionActions.enabled = false;
                    inventoryActions.enabled = false;
                    homeActions.enabled = false;
                    gestionCommandRoomPanelActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = true;
                    visitorsWaitingListActions.enabled = false;
                    uiActions.enabled = true;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
                    
                    ObjectsReference.Instance.playerController.canMove = false;
                    ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
                    break;
                case InputContext.HOME:
                    gameActions.enabled = false;
                    gestionActions.enabled = false;
                    inventoryActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = false;
                    visitorsWaitingListActions.enabled = false;
                    gestionCommandRoomPanelActions.enabled = false;
                    homeActions.enabled = true;
                    uiActions.enabled = true;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    ObjectsReference.Instance.playerController.canMove = false;
                    ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
                    break;
                case InputContext.GESTION_COMMAND_ROOM_PANEL:
                    gameActions.enabled = false;
                    gestionActions.enabled = false;
                    inventoryActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = false;
                    visitorsWaitingListActions.enabled = false;
                    homeActions.enabled = false;
                    
                    gestionCommandRoomPanelActions.enabled = true;
                    uiActions.enabled = true;
                    
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    
                    ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
                    
                    ObjectsReference.Instance.playerController.canMove = false;
                    ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
                    break;
                case InputContext.VISITOR_WAITING_LIST_MINI_GAME:
                    gameActions.enabled = false;
                    gestionActions.enabled = false;
                    inventoryActions.enabled = false;
                    homeActions.enabled = false;
                    gestionCommandRoomPanelActions.enabled = false;
                    bananaCannonMiniGameActions.enabled = false;
                    visitorsWaitingListActions.enabled = true;
                    uiActions.enabled = true;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    ObjectsReference.Instance.cameraPlayer.Set0Sensibility();

                    ObjectsReference.Instance.playerController.canMove = false;
                    ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
                    break;
            }
        }
    }
}
