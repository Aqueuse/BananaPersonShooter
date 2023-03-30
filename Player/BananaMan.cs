﻿using System.Collections;
using Audio;
using Data.Bananas;
using Enums;
using Game;
using HoaxGames;
using UI.InGame;
using UnityEngine;

namespace Player {
    public class BananaMan : MonoSingleton<BananaMan> {
        public TpsPlayerAnimator tpsPlayerAnimator;
        private PlayerController _playerController;
        private Rigidbody _rigidbody;
        private FootIK _ik;

		public BananasDataScriptableObject activeItem;
        public ItemThrowableType activeItemThrowableType = ItemThrowableType.EMPTY;
        public ItemThrowableCategory activeItemThrowableCategory = ItemThrowableCategory.EMPTY;
        
        public bool isInAir;
        public bool isInWater;
        public bool isGrabingBananaGun;
        private RagDoll _ragDoll;
        public bool isRagdoll;

        public float health;
        public float resistance;
        private int _damageCounter;

        public float lastY;
        private float _fallCounter;

        private void Start() {
            tpsPlayerAnimator = GetComponentInChildren<Animator>().GetComponent<TpsPlayerAnimator>();
            _playerController = GetComponent<PlayerController>();
            _ragDoll = gameObject.GetComponent<RagDoll>();
            _rigidbody = GetComponent<Rigidbody>();
            _ik = GetComponent<FootIK>();

			lastY = 0;
            _fallCounter = 0;
        }

        private void Update() {
            if (GameManager.Instance.gameContext == GameContext.IN_GAME) {
                isInAir = !_ik.getGroundedResult().isGrounded;
                
                if (isInAir && !isRagdoll) {
                    if (transform.position.y < lastY) {
                        _fallCounter += Time.deltaTime;
                    
                        if (_fallCounter > 1f) {
                            //Ragdoll();
                        }
                    }
                }
                else {
                    _fallCounter = 0;
                }
            }
        }

        public void IsInAir(bool isBananaManInAir) {
            isInAir = isBananaManInAir;
            tpsPlayerAnimator.IsInAir(isBananaManInAir);
            tpsPlayerAnimator.IsGrounded(!isBananaManInAir);
            lastY = transform.position.y;
        }

        private void Ragdoll() {
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
            UIFace.Instance.Die(false);
            UIFace.Instance.GetHurted(health < 50);
        }

        public void GainHealth() {
            if (activeItemThrowableCategory == ItemThrowableCategory.BANANA) {
                if (Inventory.Instance.bananaManInventory[activeItemThrowableType] > 0) {
                    health += activeItem.healthBonus;
                    resistance += activeItem.resistanceBonus;

                    Inventory.Instance.RemoveQuantity(activeItemThrowableType, 1);
                }
            }
        }

        private void TakeDamage(int damageAmount) {
            if (resistance-damageAmount > 0) {
                resistance -= damageAmount;
            }

            else {
                resistance = 0;
                health -= damageAmount;
            }
            
            if (health - damageAmount <= 0) {
                Death.Instance.Die();
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