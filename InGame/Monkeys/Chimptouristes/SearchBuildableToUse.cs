using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties.Buildables.VisitorsBuildable;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.Monkeys.Chimptouristes {
    public class SearchBuildableToUse : MonoBehaviour {
        [SerializeField] private LayerMask selectableLayerMask;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Transform _transform;
        [SerializeField] private MonkeyMenBehaviour monkeyMenBehaviour;

        private Vector3 rotatingAxis;
        private RaycastHit raycastHit;

        private TouristBehaviour touristBehaviour;
        
        public bool hasFoundBuildable;
        public BuildableBehaviour buildableFounded;

        private NavMeshPath path;

        private void OnEnable() {
            rotatingAxis = transform.TransformDirection(Vector3.up);
            touristBehaviour = GetComponent<TouristBehaviour>();
        }

        private void Update() {
            _transform.Rotate(rotatingAxis, 1);
            
            if (Physics.Raycast(origin: _transform.position, _transform.forward, maxDistance: 1000, hitInfo: out raycastHit,
                    layerMask: selectableLayerMask)) {
                if (raycastHit.transform.gameObject.TryGetComponent(out BuildableBehaviour buildable)) {

                    if (!buildable.isVisitorTargetable) return;
                    if (buildable.isBreaked) return;
                    if (buildable.isVisitorTargeted) return;
                    
                    if (touristBehaviour.visitedBuildables.Contains(buildable)) return;
                    
                    if (!CanReachBuildable(_navMeshAgent, buildable.ChimpTargetTransform.position)) return;
                    
                    foreach (var visitorNeed in monkeyMenBehaviour.sortedNeeds) {
                        var buildableData = (VisitorsBuildablePropertiesScriptableObject)buildable.buildablePropertiesScriptableObject;
                        
                        if (buildableData.needType == visitorNeed.Key) {
                            touristBehaviour.actualNeed = visitorNeed.Key;
                            buildableFounded = buildable;
                            hasFoundBuildable = true;
                        }
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