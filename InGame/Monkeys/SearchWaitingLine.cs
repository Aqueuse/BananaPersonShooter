using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.Monkeys {
    public class SearchWaitingLine : MonoBehaviour {
        [SerializeField] private LayerMask selectableLayerMask;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Transform _transform;
        [SerializeField] private MonkeyMenBehaviour monkeyMenBehaviour;

        private Vector3 rotatingAxis;
        private RaycastHit raycastHit;
        
        private NavMeshPath path;

        private void OnEnable() {
            rotatingAxis = transform.TransformDirection(Vector3.up);
        }

        private void Update() {
            _transform.Rotate(rotatingAxis, 1);
            
            if (Physics.Raycast(origin: _transform.position, _transform.forward, maxDistance: 1000, hitInfo: out raycastHit,
                    layerMask: selectableLayerMask)) {
                if (raycastHit.transform.gameObject.TryGetComponent(out BuildableBehaviour buildable)) {
                    if (buildable.isBreaked) return;
                    if (!CanReachBuildable(_navMeshAgent, buildable.ChimpTargetTransform.position)) return;
                    
                    if (buildable.buildablePropertiesScriptableObject.buildableType == BuildableType.GUICHET) {
                        monkeyMenBehaviour.monkeyMenData.destination = buildable.ChimpTargetTransform.position;
                        monkeyMenBehaviour.monkeyMenData.touristState = TouristState.GO_TO_WAITING_LINE;
                        monkeyMenBehaviour.monkeyMenData.pirateState = PirateState.GO_TO_WAITING_LINE;

                        enabled = false;
                    }
                }
            }
        }

        private bool CanReachBuildable(NavMeshAgent navMeshAgent, Vector3 buildablePosition) {
            if (NavMesh.SamplePosition(buildablePosition, out var navMeshHit, 2, 1)) {
                path = new NavMeshPath();
                if (navMeshAgent.CalculatePath(navMeshHit.position, path)) {
                    if (path.status == NavMeshPathStatus.PathComplete) {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}