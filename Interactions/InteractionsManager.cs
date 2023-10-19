using Enums;
using Interactions.InteractionsActions;
using UnityEngine;

namespace Interactions {
    /// <summary>
    /// interaction manager instance is located in the main camera
    /// </summary>
    public class InteractionsManager : MonoBehaviour {
        [SerializeField] private LayerMask itemsLayerMask;
        [SerializeField] private Transform grabbableTarget;

        public bool isGrabbing;
        private Vector3 grabbablePosition;
        private Rigidbody grabbableRigidbody;

        private GameObject _interactedObject;
        private InteractionType interactedInteractionType;

        private void Update() {
            if (!ObjectsReference.Instance.gameManager.isGamePlaying || ObjectsReference.Instance.gameManager.gameContext != GameContext.IN_GAME || isGrabbing) return;

            if (Physics.Raycast(transform.position, transform.forward,  out var raycastHit, 10, itemsLayerMask)) {
                if (_interactedObject != null) _interactedObject.GetComponent<Interaction>().Desactivate(); // magic ( ͡• ͜ʖ ͡• )
                _interactedObject = raycastHit.transform.gameObject;
                _interactedObject.GetComponent<Interaction>().Activate();
            }
            else {
                if (_interactedObject != null) {
                    _interactedObject.GetComponent<Interaction>().Desactivate();
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
            if (_interactedObject == null || isGrabbing) return;
        
            interactedInteractionType = _interactedObject.GetComponent<Interaction>().interactionType;

            switch (interactedInteractionType) {
                case InteractionType.DOOR_BEETWEEN_LEVELS:
                    DoorBeetweenLevelsInteraction.Activate(_interactedObject);
                    break;
                case InteractionType.BUBBLE:
                    MiniChimpInteraction.Activate(_interactedObject);
                    break;
                case InteractionType.BANANAGUN:
                    BananaGunInteraction.Activate(_interactedObject);
                    break;
                case InteractionType.COMMAND_ROOM_PANEL:
                    CommandRoomPanelInteraction.Activate(_interactedObject);
                    break;
                case InteractionType.BANANA_CANNON_MINI_GAME:
                    BananaCannonMiniGameInteraction.Activate();
                    break;
                case InteractionType.RETRIEVER:
                    RetrieverInteraction.Activate(_interactedObject);
                    break;
                case InteractionType.BLUEPRINTS_DATA:
                    _interactedObject.GetComponent<BlueprintsDataInteraction>().Activate();
                    break;
                case InteractionType.TELEPORT_TO_PORTAL_DESTINATION:
                    _interactedObject.GetComponent<PortalDestinationInteraction>().Activate();
                    break;
                case InteractionType.VERTICAL_PROPULSOR:
                    _interactedObject.GetComponent<VerticalPropulsorInteraction>().Activate();
                    break;
            }
        }

        public void Grab() {
            if (_interactedObject == null) return;
            if (_interactedObject.GetComponent<Interaction>().interactionType != InteractionType.GRABBABLE_PIECE) return;
        
            grabbableRigidbody = _interactedObject.GetComponent<Rigidbody>();
            grabbableRigidbody.useGravity = false;
                    
            _interactedObject.GetComponent<Interaction>().Desactivate();

            isGrabbing = true;
            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.GRAB_SOMETHING, 0);
        }

        public void Release() {
            // let the object fall
            if (_interactedObject == null) return;
        
            if (_interactedObject.GetComponent<Interaction>().interactionType == InteractionType.GRABBABLE_PIECE) {
                grabbableRigidbody = _interactedObject.GetComponent<Rigidbody>();
                grabbableRigidbody.useGravity = true;
                grabbableRigidbody.velocity = Vector3.zero;

                _interactedObject.GetComponent<Interaction>().Desactivate();
                _interactedObject = null;
                isGrabbing = false;
            }
        }
    }
}