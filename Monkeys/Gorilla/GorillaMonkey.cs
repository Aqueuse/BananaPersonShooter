using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Monkeys.Gorilla {
    enum GorillaAttackType {
        SHOCKWAVE = 0,
        CATCHPLAYER = 1,
        TOURBISMASH = 2
    }

    public class GorillaMonkey : MonoBehaviour {
        [SerializeField] private GameObject shockWavePrefab;
        public Transform gorillaHandRight;
        public Transform gorillaHandLeft;
        private NavMeshAgent _navMeshAgent;
        private Monkey _monkey;

        private Vector3 _gorillaHandLeftPosition;
        private Vector3 _gorillaHandRightPosition;

        private Animator _animator;

        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;
        private Vector3 _bananaManPosition;

        public bool isAttackingPlayer;

        private int _randomAttack;
        float _combatPhaseFactor;
        private GorillaAttackType _gorillaAttackType;

        // animations
        private static readonly int Shockwave = Animator.StringToHash("SHOCKWAVE");
        private static readonly int Swip = Animator.StringToHash("SWIP");
        private static readonly int Punch = Animator.StringToHash("PUNCH");
        private static readonly int PunchRight = Animator.StringToHash("PUNCH RIGHT");
        private static readonly int SwipRight = Animator.StringToHash("SWIP RIGHT");
        private static readonly int Tourbismash = Animator.StringToHash("TOURBISMASH");
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");
        private static readonly int CanMove = Animator.StringToHash("canMove");
        private static readonly int Roar = Animator.StringToHash("ROAR");
        private static readonly int Flex = Animator.StringToHash("FLEX");

        // syncrhonize navmeshagent with animator
        private Transform _transform;
        private float _worldDeltaPositionX;
        private float _worldDeltaPositionY;
        private Vector2 _deltaPosition;
        private Vector3 _worldDeltaPosition;
        private bool _shouldMove;
        private float _smooth;

        private List<int> _nearPlayerAttack;
        private List<int> _mediumPlayerAttack;
        private List<int> _farPlayerAttack;

        private void Start() {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _monkey = GetComponent<Monkey>();

            _nearPlayerAttack = new List<int> { Roar, Flex, Tourbismash };
            _mediumPlayerAttack = new List<int> { Punch, PunchRight, Swip, SwipRight };
            _farPlayerAttack = new List<int> { Roar, Flex, Shockwave, Shockwave, Shockwave };

            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;

            _navMeshAgent.velocity = new Vector3(4, 4, 4);
        }

        private void Update() {
            if (ObjectsReference.Instance.gameManager.isGamePlaying) {
                if (_monkey.monkeyState == MonkeyState.ANGRY) {
                    var bananaManPosition = ObjectsReference.Instance.bananaMan.transform.position;
                    Vector3 bananaManPositionXY = new Vector3(bananaManPosition.x, 0, bananaManPosition.z);

                    if (_navMeshAgent.isOnNavMesh) {
                        _navMeshAgent.SetDestination(bananaManPositionXY);

                        if (_navMeshAgent.remainingDistance <= 7) _gorillaAttackType = GorillaAttackType.TOURBISMASH;
                        if (_navMeshAgent.remainingDistance is > 7 and <= 10)
                            _gorillaAttackType = GorillaAttackType.CATCHPLAYER;
                        if (_navMeshAgent.remainingDistance >= 13) _gorillaAttackType = GorillaAttackType.SHOCKWAVE;
                    }

                    if (!isAttackingPlayer) {
                        isAttackingPlayer = true;
                        switch (_gorillaAttackType) {
                            case GorillaAttackType.TOURBISMASH:
                                Invoke(nameof(TourbiSmash), Random.Range(0.5f, 1));
                                break;
                            case GorillaAttackType.CATCHPLAYER:
                                Invoke(nameof(CatchPlayer), Random.Range(1, 3));
                                break;
                            case GorillaAttackType.SHOCKWAVE:
                                // phase 1 : one wave
                                Invoke(nameof(ShockWavePlayer), Random.Range(1, 3));
                                // phase 2 : two waves (left hand, right hand)
                                // phase 3 : one wave left, move, on wave right
                                break;
                        }
                    }

                    SynchronizeAnimatorAndAgent();
                }

                if (_monkey.monkeyState == MonkeyState.SAD) {
                    SynchronizeAnimatorAndAgent();
                    if (_navMeshAgent.remainingDistance < 10) {
                        _navMeshAgent.SetDestination(RandomNavmeshLocation(1000));
                    }
                }
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

        void TourbiSmash() {
            _randomAttack = Random.Range(0, _nearPlayerAttack.Count);
            _animator.SetTrigger(_nearPlayerAttack[_randomAttack]);
        }

        public void CatchPlayer() {
            _randomAttack = Random.Range(0, _mediumPlayerAttack.Count);
            _animator.SetTrigger(_mediumPlayerAttack[_randomAttack]);
        }

        void ShockWavePlayer() {
            _randomAttack = Random.Range(0, _farPlayerAttack.Count);
            _animator.SetTrigger(_farPlayerAttack[_randomAttack]);
        }

        public void CreateShockWave() {
            _gorillaHandLeftPosition = gorillaHandLeft.position;
            _gorillaHandRightPosition = gorillaHandRight.position;

            var leftSpawnPosition = new Vector3(_gorillaHandLeftPosition.x, 0.5f, _gorillaHandLeftPosition.z);

            Instantiate(shockWavePrefab, leftSpawnPosition, Quaternion.identity);
        }

        /////  DAMAGES ////

        public void BeAttracted() {
        }


        private void OnTriggerEnter(Collider other) {
            GetComponent<Monkey>().Feed(ObjectsReference.Instance.bananaMan.activeItem.sasiety);
        }

        private Vector3 RandomNavmeshLocation(float radius) {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit navMeshHit, radius, 1)) {
                finalPosition = navMeshHit.position;
            }

            return finalPosition;
        }
    }
}