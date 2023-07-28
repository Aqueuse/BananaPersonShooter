using UnityEngine;
using Items.ItemsActions;

namespace Items {
    public class ItemsManager : MonoBehaviour {
        [SerializeField] private LayerMask itemsLayerMask;
        [SerializeField] private Transform grabbableTarget;
        
        public bool isGrabbing;
        private Vector3 grabbablePosition;
        private Rigidbody grabbableRigidbody;
        
        private GameObject _interactedObject;
        private ItemStaticType _interactedItemStaticType;
        
        private void Update() {
            if (!ObjectsReference.Instance.gameManager.isGamePlaying || ObjectsReference.Instance.gameManager.gameContext != GameContext.IN_GAME || isGrabbing) return;

            if (Physics.Raycast(transform.position, transform.forward,  out var raycastHit, 10, itemsLayerMask)) {
                if (_interactedObject != null) _interactedObject.GetComponent<ItemStatic>().Desactivate(); // magic ( ͡• ͜ʖ ͡• )
                _interactedObject = raycastHit.transform.gameObject;
                _interactedObject.GetComponent<ItemStatic>().Activate();
            }
            else {
                if (_interactedObject != null) {
                    _interactedObject.GetComponent<ItemStatic>().Desactivate();
                    _interactedObject = null;
                }
            }
        }

        private void FixedUpdate() {
            if (isGrabbing) {
                grabbableRigidbody.velocity = (grabbableTarget.position - _interactedObject.transform.position) * 10;
            }
        }

        public void Validate() {
            if (_interactedObject != null && !isGrabbing) {
                _interactedItemStaticType = _interactedObject.GetComponent<ItemStatic>().itemStaticType;

                switch (_interactedItemStaticType) {
                    case ItemStaticType.DOOR_BEETWEEN_LEVELS:
                        DoorBeetweenLevelsItemAction.Activate(_interactedObject);
                        break;
                    case ItemStaticType.MINI_CHIMP_COMMAND_ROOM:
                        MiniChimpItemAction.Activate(_interactedObject);
                        break;
                    case ItemStaticType.BANANAGUN:
                        BananaGunItemAction.Activate(_interactedObject);
                        break;
                    case ItemStaticType.COMMAND_ROOM_PANEL:
                        CommandRoomPanelItemAction.Activate(_interactedObject);
                        break;
                    case ItemStaticType.BANANA_CANNON_MINI_GAME:
                        BananaCannonMiniGameItemAction.Activate();
                        break;
                    case ItemStaticType.RETRIEVER:
                        RetrieverItemAction.Activate(_interactedObject);
                        break;
                    case ItemStaticType.BLUEPRINTS_DATA:
                        BlueprintsData.Activate();
                        break;
                    case ItemStaticType.PORTAL_DESTINATION:
                        _interactedObject.GetComponent<PortalDestinationItemAction>().Activate();
                        break;
                }
            }
        }

        public void Grab() {
            if (_interactedObject != null) {
                if (_interactedObject.GetComponent<ItemStatic>().itemStaticType == ItemStaticType.GRABBABLE_PIECE) {
                    grabbableRigidbody = _interactedObject.GetComponent<Rigidbody>();
                    grabbableRigidbody.useGravity = false;
                    
                    _interactedObject.GetComponent<ItemStatic>().Desactivate();

                    isGrabbing = true;
                    ObjectsReference.Instance.audioManager.PlayEffect(EffectType.GRAB_SOMETHING, 0);
                }
            }
        }

        public void Release() {
            // let the object fall
            if (_interactedObject != null) {
                if (_interactedObject.GetComponent<ItemStatic>().itemStaticType == ItemStaticType.GRABBABLE_PIECE) {
                    grabbableRigidbody = _interactedObject.GetComponent<Rigidbody>();
                    grabbableRigidbody.useGravity = true;
                    grabbableRigidbody.velocity = Vector3.zero;

                    _interactedObject.GetComponent<ItemStatic>().Desactivate();
                    _interactedObject = null;
                    isGrabbing = false;
                }
            }
        }
    } 
}