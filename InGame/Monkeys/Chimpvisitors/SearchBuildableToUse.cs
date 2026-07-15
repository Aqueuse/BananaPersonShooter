using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties.Buildables.VisitorsBuildable;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.Monkeys.Chimpvisitors {
    public class SearchBuildableToUse : MonoBehaviour {
        [SerializeField] private LayerMask selectableLayerMask;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private VisitorBehaviour visitorBehaviour;
        [SerializeField] private VisitorStateMachine _visitorStateMachine;

        private Vector3 _rotatingAxis;
        private RaycastHit _raycastHit;
        
        private NavMeshPath _navMeshPath;

        private void OnEnable() {
            _rotatingAxis = transform.TransformDirection(Vector3.up);
            
            Invoke(nameof(StopSearching), 30);
        }

        private void Update() {
            transform.Rotate(_rotatingAxis, 1);
            
            if (Physics.Raycast(origin: transform.position, transform.forward, maxDistance: 1000, hitInfo: out _raycastHit,
                    layerMask: selectableLayerMask)) {
                
                if (_raycastHit.transform.gameObject.TryGetComponent(out BuildableBehaviour buildable)) {
                    var buildableData = (VisitorsBuildablePropertiesScriptableObject)buildable.buildablePropertiesScriptableObject;
                    var needLocation = buildable.transform.GetComponent<VisitorBuildableBehaviour>();

                    if (!buildable.isVisitorTargetable) return;
                    if (buildable.isVisitorTargeted) return;
                    if (!CanReachBuildable(buildable.ChimpTargetTransform.position)) return;
                    if (buildableData.needType != visitorBehaviour.monkeyMenData.need) return;
                    if (needLocation.isOccupied) return;
                    
                    needLocation.enabled = true;
                    needLocation.PrepareOccupation(visitorBehaviour.navMeshAgent, visitorBehaviour.monkeyMenData.need == NeedType.PILLAGE);
                    
                    _visitorStateMachine.touristState = TouristState.GO_FILL_NEED;
                    visitorBehaviour.SetDestination(buildable.transform.position);
                    
                    enabled = false;
                }
            }
        }

        private bool CanReachBuildable(Vector3 buildablePosition) {
            if (NavMesh.SamplePosition(buildablePosition, out var navMeshHit, 2, 1)) {
                _navMeshPath = new NavMeshPath();
                if (navMeshAgent.CalculatePath(navMeshHit.position, _navMeshPath)) {
                    if (_navMeshPath.status == NavMeshPathStatus.PathComplete) {
                        return true;
                    }
                }
            }

            return false;
        }

        private void StopSearching() {
            _visitorStateMachine.searchMapToVisit();
        }
    }
}