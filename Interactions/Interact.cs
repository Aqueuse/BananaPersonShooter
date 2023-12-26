using UI.InGame;
using UnityEngine;

namespace Interactions {
    /// interaction manager instance is located in the main camera
    
    public class Interact : MonoBehaviour {
        [SerializeField] private LayerMask itemsLayerMask;
        [SerializeField] private GenericDictionary<InteractionType, Interaction> interactClassByInteractionType;

        public GameObject _interactedObject;
        private InteractionType interactedInteractionType;
        
        private void Update() {
            if (!ObjectsReference.Instance.gameManager.isGamePlaying || ObjectsReference.Instance.gameManager.gameContext != GameContext.IN_GAME) return;

            if (Physics.Raycast(transform.position, transform.forward,  out var raycastHit, 10, itemsLayerMask)) {
                if (raycastHit.transform.gameObject.GetComponent<UInteraction>() == null) return;

//                if (_interactedObject != null) _interactedObject.GetComponent<UInteraction>().Desactivate(); // magic ( ͡• ͜ʖ ͡• )
                _interactedObject = raycastHit.transform.gameObject;
                _interactedObject.GetComponent<UInteraction>().Activate();
            }
            else {
                if (_interactedObject == null) return;
                _interactedObject.GetComponent<UInteraction>().Desactivate();
                _interactedObject = null;
            }
        }

        public void Validate() {
            if (_interactedObject == null) return;

            interactedInteractionType = _interactedObject.GetComponent<UInteraction>().interactionType;

            interactClassByInteractionType[interactedInteractionType].Activate(_interactedObject);
        }
    }
}