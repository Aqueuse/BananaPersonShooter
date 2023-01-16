using System.Collections;
using Audio;
using Cameras;
using Enums;
using UI.InGame;
using UnityEngine;

namespace Player {
    class BananaMan : MonoSingleton<BananaMan> {
        public TpsPlayerAnimator tpsPlayerAnimator;
        private PlayerController _playerController;
        private Rigidbody _rigidbody;

        public BananasDataScriptableObject activeItem;
        public ItemThrowableType activeItemThrowableType = ItemThrowableType.ROCKET;
        public ItemThrowableCategory activeItemThrowableCategory = ItemThrowableCategory.ROCKET;
        
        public bool isInAir;
        public bool isInWater;
        public bool isGrabingMover;
        private RagDoll _ragDoll;
        public bool isRagdoll;

        public float health;
        public float resistance;
        public bool hasMover;
        public AdvancementType advancementType = AdvancementType.FIRST_MINICHIMP_INTERACT;
        private int _damageCounter;

        public float lastY;
        private float _fallCounter;

        private void Start() {
            tpsPlayerAnimator = GetComponentInChildren<Animator>().GetComponent<TpsPlayerAnimator>();
            _playerController = GetComponent<PlayerController>();
            _ragDoll = gameObject.GetComponent<RagDoll>();
            _rigidbody = GetComponent<Rigidbody>();
            
            lastY = 0;
            _fallCounter = 0;
        }

        private void Update() {
            if (GameManager.Instance.isInGame) {
                if (isInAir && !isRagdoll) {
                    if (transform.position.y < lastY) {
                        _fallCounter+= Time.deltaTime;
                    
                        if (_fallCounter > 1f) {
                            Ragdoll();
                        }
                    }
                }
                else {
                    _fallCounter = 0;
                }
            }
        }
        
        public void Ragdoll() {
            _damageCounter = 0;
            isRagdoll = true;

            tpsPlayerAnimator.enabled = false;
            _ragDoll.SetRagDoll(true);
            UIFace.Instance.GetGooglyEyes();

            StartCoroutine(SetPlayerBackFromRagdoll());
        }

        public void RagdollAgainstCollider(Collider objectCollider, float force) {
            if (!_playerController.isRolling) {
                _damageCounter = 0;
                isRagdoll = true;

                tpsPlayerAnimator.enabled = false;
                _ragDoll.SetRagDoll(true);
                UIFace.Instance.GetGooglyEyes();

                _rigidbody.AddForce(force*(transform.position - objectCollider.transform.position), ForceMode.Impulse);

                StartCoroutine(SetPlayerBackFromRagdoll());
            }
        }
        
        IEnumerator SetPlayerBackFromRagdoll() {
            yield return new WaitForSeconds(1.5f);

            _ragDoll.SetRagDoll(false);

            tpsPlayerAnimator.enabled = true;
            tpsPlayerAnimator.GetUp();
            isRagdoll = false;

            yield return new WaitForSeconds(0.7f);
            
            TakeDamage(_damageCounter);
            
            yield return null;
        }
        
        public void ResetToPlayable() {
            _ragDoll.SetRagDoll(false);
            isRagdoll = false;
            _playerController.isRolling = false;
            _playerController.canMove = true;

            tpsPlayerAnimator.enabled = true;
            _playerController.mainCameraTransform.GetComponent<ThirdPersonOrbitCamBasic>().enabled = true;
            UIFace.Instance.Die(false);
            UIFace.Instance.GetHurted(health < 50);
        }

        public void GainHealth() {
            if (activeItemThrowableCategory == ItemThrowableCategory.BANANA) {
                if (Inventory.Instance.bananaManInventory[activeItemThrowableType] > 0) {
                    health += activeItem.healthBonus;
                    resistance += activeItem.resistanceBonus;

                    Inventory.Instance.RemoveQuantity(activeItemThrowableType, 1);
            
                    UIVitals.Instance.Set_Health(health);
                    UIVitals.Instance.Set_Resistance(resistance);
                }
            }
        }

        public void TakeDamage(int damageAmount) {
            if (resistance-damageAmount > 0) {
                resistance -= damageAmount;
            }

            else {
                resistance = 0;
                health -= damageAmount;
            }
            
            UIVitals.Instance.Set_Health(health);
            UIVitals.Instance.Set_Resistance(resistance);

            if (health - damageAmount <= 0) {
                UIVitals.Instance.Set_Health(0);
                GameManager.Instance.Death();
            }
            
            UIFace.Instance.GetHurted(health < 50);
        }

        public void Die() {
            isRagdoll = true;
            tpsPlayerAnimator.enabled = false;

            _playerController.canMove = false;
            _ragDoll.SetRagDoll(true);
        }

        public void PlayFootstep() {
            AudioManager.Instance.PlayFootStepOneShot();
        }

    }
}