using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours.PlateformsEffects;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.Monkeys.Chimpirates {
    public class PirateCollisions : MonoBehaviour {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Rigidbody pirateRigidbody;
        [SerializeField] private PirateBehaviour pirateBehaviour;
        
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer != 7) return;

            if (other.TryGetComponent(out PlateformBehaviour plateformBehaviour)) {
                switch (plateformBehaviour.plateformType) {
                    case BananaType.CAVENDISH:
                        pirateBehaviour.pirateSavedData.pirateState = PirateState.PLATEFORM_INTERACTION;

                        plateformBehaviour.isPirateTargeted = false;
                        _navMeshAgent.enabled = false;
                        pirateRigidbody.isKinematic = false;
                        pirateRigidbody.useGravity = true;
                        GetComponent<CapsuleCollider>().isTrigger = false;
                        
                        pirateBehaviour.enabled = false;

                        plateformBehaviour.GetComponent<UpEffect>().JumpMe(pirateRigidbody, 20000);
                        pirateBehaviour.Fly();

                        break;
                    default:
                        return;
                }
            }
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.layer != 11) return;

            if (pirateBehaviour.pirateSavedData.pirateState == PirateState.PLATEFORM_INTERACTION) {
                pirateRigidbody.isKinematic = true;
                pirateRigidbody.useGravity = false;
                _navMeshAgent.enabled = true;

                pirateBehaviour.enabled = true;
                pirateBehaviour.Flee();
            }
        }
    }
}
