using UI.InGame;
using UnityEngine;

namespace InGame.Interactions {
    public class Grab : MonoBehaviour {
        [SerializeField] private Transform grabbableTarget;
        [SerializeField] private LayerMask itemsLayerMask;

        public bool isGrabbing;
        private Vector3 grabbablePosition;
        private Rigidbody grabbableRigidbody;

        private GameObject grabbedObject;

        private void Update() {
            if (ObjectsReference.Instance.gameManager.gameContext != GameContext.IN_GAME || isGrabbing) return;

            if (Physics.Raycast(transform.position, transform.forward, out var raycastHit, 10, itemsLayerMask)) {
                if (raycastHit.transform.gameObject.TryGetComponent(out Grabbable grabbable)) {
                    if (grabbedObject != null) grabbable.GetComponentInChildren<UInteraction>().Desactivate(); // magic ( ͡• ͜ʖ ͡• )
                     grabbedObject = grabbable.transform.gameObject;
                     grabbedObject.GetComponentInChildren<UInteraction>().Activate();
                }
            }
            else {
                if (grabbedObject == null) return;
                grabbedObject.GetComponentInChildren<UInteraction>().Desactivate();
                grabbedObject = null;

            }
        }

        private void FixedUpdate() {
            if (isGrabbing) {
                grabbableRigidbody.velocity = (grabbableTarget.position - grabbedObject.transform.position) * 10;
            }
        }

        public void DoGrab() {
            if (grabbedObject == null) return;

            grabbableRigidbody = grabbedObject.GetComponent<Rigidbody>();
            grabbableRigidbody.useGravity = false;
            grabbableRigidbody.isKinematic = false;

            grabbedObject.GetComponentInChildren<UInteraction>().Desactivate();

            isGrabbing = true;
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.GRAB_SOMETHING, 0);
        }

        public void Release() {
            // let the object fall
            if (grabbedObject == null) return;

            grabbableRigidbody = grabbedObject.GetComponent<Rigidbody>();
            grabbableRigidbody.useGravity = true;
            grabbableRigidbody.velocity = Vector3.zero;

            grabbedObject.GetComponentInChildren<UInteraction>().Desactivate();
            grabbedObject = null;
            isGrabbing = false;
        }
    }
}