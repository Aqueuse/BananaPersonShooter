using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.Monkeys.Chimpirates {
    public class SearchBuildableToBreak : MonoBehaviour {
        [SerializeField] private LayerMask selectableLayerMask;
        [SerializeField] private NavMeshAgent _navMeshAgent;
    
        private Vector3 rotatingAxis;
        private RaycastHit raycastHit;
        private Transform _transform;

        public bool hasFoundBuildable;
        public BuildableBehaviour buildableFounded;
        
        private NavMeshPath path;
        
        private void Start() {
            _transform = transform;
            rotatingAxis = transform.TransformDirection(Vector3.up);
            path = new NavMeshPath();
        }

        private void Update() {
            _transform.Rotate(rotatingAxis, 1);
            
            if (Physics.Raycast(origin: _transform.position, _transform.forward, maxDistance:1000, hitInfo: out raycastHit, layerMask:selectableLayerMask)) {
                
                if (raycastHit.transform.gameObject.TryGetComponent(out BuildableBehaviour buildable)) {
                    if (!buildable.isBreaked && !buildable.isPirateTargeted && CanReachBuildable(_navMeshAgent, buildable.transform.position)) {
                        buildableFounded = buildable;
                        hasFoundBuildable = true;
                    }
                }
            }
        }
        
        private bool CanReachBuildable(NavMeshAgent navMeshAgent, Vector3 buildablePosition) {
            if (NavMesh.SamplePosition(buildablePosition, out var navMeshHit, 2, 1)) {
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
