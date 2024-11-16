using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace InGame.Monkeys.Ancestors.Gorilla {
    public class GorillaMonkey : MonoBehaviour {
        private NavMeshAgent _navMeshAgent;
        [SerializeField] private Monkey _monkey;
        
        private Vector3 _gorillaHandLeftPosition;

        private Animator _animator;

        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;
        private Vector3 bananaManPosition;
        private Vector3 bananaManPositionXY;
        
        // animations
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");
        private static readonly int ShouldMove = Animator.StringToHash("shouldMove");

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

        private NavMeshPath path;
        
        private void Start() {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _transform = transform;
            
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;

            _navMeshAgent.velocity = new Vector3(4, 4, 4);
        }

        private void Update() {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME) {
                if (_monkey.smelledBananasOnBananaMan) {
                    bananaManPosition = ObjectsReference.Instance.bananaMan.transform.position;

                    _navMeshAgent.SetDestination(bananaManPosition);
                    
                    bananaManPositionXY = new Vector3(bananaManPosition.x, transform.position.y, bananaManPosition.z);
                    if (_navMeshAgent.remainingDistance < 5) transform.LookAt(bananaManPositionXY);
                }

                else {
                    if (_navMeshAgent.remainingDistance < 10) {
                        _navMeshAgent.SetDestination(GetRandomNavmeshLocation(1000));
                    }
                }
                
                SynchronizeAnimatorAndAgent();
            }
        }

        private void OnAnimatorMove() {
            if (_navMeshAgent != null) {
                transform.position = _navMeshAgent.nextPosition;
            }
        }

        private void SynchronizeAnimatorAndAgent() {
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

            _shouldMove = _velocity.magnitude > 0.5f & _navMeshAgent.remainingDistance > _navMeshAgent.radius;

            // Update animation parameters
            _animator.SetBool(ShouldMove, _shouldMove);
            _animator.SetFloat(VelocityX, _velocity.x);
            _animator.SetFloat(VelocityZ, _velocity.y);
        }
        
        private Vector3 GetRandomNavmeshLocation(float radius) {
            randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;
            finalPosition = Vector3.zero;
            
            if (NavMesh.SamplePosition(randomDirection, out var navMeshHit, radius, 1)) {
                finalPosition = navMeshHit.position;
            }

            return finalPosition;
        }
    }
}