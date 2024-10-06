using UnityEngine;
using UnityEngine.AI;

namespace InGame.Monkeys.Chimployees {
    public class ChimployeeMovement : MonoBehaviour {
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;

        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;
        
        // animations
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");
        private static readonly int CanMove = Animator.StringToHash("canMove");

        // syncrhonize navmeshagent with animator
        private Transform _transform;
        private float _worldDeltaPositionX;
        private float _worldDeltaPositionY;
        private Vector2 _deltaPosition;
        private Vector3 _worldDeltaPosition;
        private bool _shouldMove;
        private float _smooth;

        private Vector3 randomDirection;
        private Vector3 finalPosition;

        private void Start() {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();

            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;

            _navMeshAgent.velocity = new Vector3(4, 4, 4);
        }

        private void Update() {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME) {
                SynchronizeAnimatorAndAgent();
            }
        }

        private void OnAnimatorMove() {
            if (_navMeshAgent != null) {
                transform.position = _navMeshAgent.nextPosition;
            }
        }

        private void SynchronizeAnimatorAndAgent() {
            _transform = transform;
            _worldDeltaPosition = _navMeshAgent.nextPosition - _transform.position;

            // Map 'worldDeltaPosition' to local space
            _worldDeltaPositionX = Vector3.Dot(_transform.right, _worldDeltaPosition);
            _worldDeltaPositionY = Vector3.Dot(_transform.forward, _worldDeltaPosition);
            _deltaPosition = new Vector2(_worldDeltaPositionX, _worldDeltaPositionY);

            // Low-pass filter the deltaMove
            _smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, _deltaPosition, _smooth);

            // Update velocity if time advances
            if (Time.deltaTime > 1e-5f) _velocity = _smoothDeltaPosition / Time.deltaTime;

            _shouldMove = _velocity.magnitude > 0.5f && _navMeshAgent.remainingDistance > _navMeshAgent.radius;

            // Update animation parameters
            _animator.SetBool(CanMove, _shouldMove);
            _animator.SetFloat(VelocityX, _velocity.x);
            _animator.SetFloat(VelocityZ, _velocity.y);
        }
    }
}