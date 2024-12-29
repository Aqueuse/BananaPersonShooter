using UnityEngine;

namespace InGame.Interactions {
    public class Grab : MonoBehaviour {
        [SerializeField] private Transform grabbableTarget;
        [SerializeField] private LayerMask itemsLayerMask;

        public bool isGrabbing;
        
        private GameObject grabbableObject;
        private Rigidbody grabbedRigidbody;
        private Grabbable grabbableClass;

        private RaycastHit raycastHit;
        
        private void Update() {
            if (isGrabbing) return;

            if (Physics.Raycast(transform.position, transform.forward, out raycastHit, 10, itemsLayerMask)) {
                if (raycastHit.transform.gameObject.TryGetComponent(out Grabbable grabbable)) {
                    if (grabbableObject != null) grabbable.uInteraction.Desactivate(); // magic ( ͡• ͜ʖ ͡• )
                     grabbableObject = grabbable.transform.gameObject;
                     grabbableClass = grabbable;
                     
                     grabbableClass.uInteraction.Activate();
                }
            }
            else {
                if (grabbableObject == null) return;
                
                grabbableClass.uInteraction.Desactivate();
                grabbableObject = null;

            }
        }

        private void FixedUpdate() {
            if (isGrabbing) {
                grabbedRigidbody.velocity = (grabbableTarget.position - grabbableObject.transform.position) * 10;
            }
        }

        public void DoGrab() {
            if (grabbableObject == null) return;

            grabbedRigidbody = grabbableObject.GetComponent<Rigidbody>();
            grabbedRigidbody.useGravity = false;
            grabbedRigidbody.isKinematic = false;

            grabbableClass.uInteraction.Desactivate();

            isGrabbing = true;
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.GRAB_SOMETHING, 0);
        }

        public void Release() {
            // let the object fall
            if (grabbableObject == null) return;

            grabbedRigidbody = grabbableObject.GetComponent<Rigidbody>();
            grabbedRigidbody.useGravity = true;
            grabbedRigidbody.velocity = Vector3.zero;

            grabbableClass.uInteraction.Desactivate();
            grabbableObject = null;
            isGrabbing = false;
        }
    }
}