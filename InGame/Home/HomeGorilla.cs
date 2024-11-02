using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace InGame.Home {
public class HomeGorilla : MonoBehaviour {
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");
        private static readonly int ShouldMove = Animator.StringToHash("shouldMove");

        private NavMeshAgent _homeGorillaNavMeshAgent;
        private Animator _homeGorillaAnimator;

        // syncrhonize navmeshagent with animator
        private Transform _transform;
        private float _worldDeltaPositionX;
        private float _worldDeltaPositionY;
        private Vector2 _deltaPosition;
        private Vector3 _worldDeltaPosition;
        private bool _shouldMove;
        private float _smooth;

        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;
        
        private void Start() {
            _homeGorillaNavMeshAgent = GetComponent<NavMeshAgent>();
            _homeGorillaAnimator = GetComponent<Animator>();
            
            _homeGorillaNavMeshAgent.updatePosition = false;
            _homeGorillaNavMeshAgent.updateRotation = true;

            _homeGorillaNavMeshAgent.velocity = new Vector3(4, 4, 4);
        }

        private void Update() {
            if (_homeGorillaNavMeshAgent.isOnNavMesh) {
                SynchronizeAnimatorAndAgent();

                if (_homeGorillaNavMeshAgent.remainingDistance < 10) {
                    _homeGorillaNavMeshAgent.SetDestination(RandomNavmeshLocation(1000));
                }
            }
        }
        
        private void OnAnimatorMove() {
            transform.position = _homeGorillaNavMeshAgent.nextPosition;
        }
        
        private void SynchronizeAnimatorAndAgent() {
            _transform = transform;
            _worldDeltaPosition = _homeGorillaNavMeshAgent.nextPosition - _transform.position;

            // Map 'worldDeltaPosition' to local space
            _worldDeltaPositionX = Vector3.Dot(_transform.right, _worldDeltaPosition);
            _worldDeltaPositionY = Vector3.Dot(_transform.forward, _worldDeltaPosition);
            _deltaPosition = new Vector2(_worldDeltaPositionX, _worldDeltaPositionY);

            // Low-pass filter the deltaMove
            _smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, _deltaPosition, _smooth);

            // Update velocity if time advances
            if (Time.deltaTime > 1e-5f) _velocity = _smoothDeltaPosition / Time.deltaTime;

            _shouldMove = _velocity.magnitude > 0.5f && _homeGorillaNavMeshAgent.remainingDistance > _homeGorillaNavMeshAgent.radius;

            // Update animation parameters
            _homeGorillaAnimator.SetBool(ShouldMove, _shouldMove);
            _homeGorillaAnimator.SetFloat(VelocityX, _velocity.x);
            _homeGorillaAnimator.SetFloat(VelocityZ, _velocity.y);
        }
        
        private Vector3 RandomNavmeshLocation(float radius) {
            var randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;
            var finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out var navMeshHit, radius, 1)) {
                finalPosition = navMeshHit.position;
            }
            return finalPosition;
        }
    }
}
