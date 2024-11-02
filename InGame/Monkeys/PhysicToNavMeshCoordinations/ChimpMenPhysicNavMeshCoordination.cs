using InGame.Monkeys.Chimpirates;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.Monkeys.PhysicToNavMeshCoordinations {
    public enum PhysicNavmeshCoordinationState {
        NAVMESH,
        PHYSIC
    }
    
    public class ChimpMenPhysicNavMeshCoordination : PhysicNavMeshCoordination {
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Rigidbody _rigidbody;

        // synchronize navmeshagent with animator
        private static readonly int VelocityX = Animator.StringToHash("XVelocity");
        private static readonly int VelocityZ = Animator.StringToHash("ZVelocity");
        private static readonly int isInAirAnimatorProperty = Animator.StringToHash("isInAir");

        public PhysicNavmeshCoordinationState physicNavmeshCoordinationState;
        
        public override void SwitchToPhysic() {
            physicNavmeshCoordinationState = PhysicNavmeshCoordinationState.PHYSIC;
            
            animator.SetFloat(VelocityX, 0);
            animator.SetFloat(VelocityZ, 0);
            animator.SetBool(isInAirAnimatorProperty, true);

            _navMeshAgent.enabled = false;
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            GetComponent<CapsuleCollider>().isTrigger = false;

            GetComponent<PirateBehaviour>().monkeyMenBehaviour.monkeyMenData.pirateState = PirateState.PLATEFORM_INTERACTION;
            
            
        }

        public override void SwitchToNavMeshAgent() {
            physicNavmeshCoordinationState = PhysicNavmeshCoordinationState.NAVMESH;

            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
            _navMeshAgent.enabled = true;

            animator.SetBool(isInAirAnimatorProperty, false);

            GetComponent<CapsuleCollider>().isTrigger = false;
            GetComponent<PirateBehaviour>().monkeyMenBehaviour.monkeyMenData.pirateState = PirateState.GO_BACK_TO_TELEPORTER;
        }
    }
}
