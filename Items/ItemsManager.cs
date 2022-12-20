﻿using Bosses;
using Enums;
using UI.InGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Items {
    public class ItemsManager : MonoSingleton<ItemsManager> {
        private GameObject _interactedObject;
        private LayerMask _itemsLayerMask;

        public GameObject lootMessage;

        private void Start() {
            _itemsLayerMask = LayerMask.NameToLayer("Items");
        }

        void Interact(GameObject objectInteracted) {
            _interactedObject = objectInteracted;
        }

        public void Validate(InputAction.CallbackContext context) {
            if (context.performed) {
                if (_interactedObject != null) {
                    ItemType itemType = _interactedObject.GetComponent<Item>().itemType;

                    switch (itemType) {
                        case ItemType.DOOR:
                            Transform spawnPoint = _interactedObject.GetComponent<Door>().spawnPoint;
                            GameManager.Instance.SwitchScene(_interactedObject.GetComponent<Door>().destinationMap,
                                spawnPoint.position);
                            LeaveInteraction();
                            break;
                        case ItemType.REGIME:
                            var bananaType = _interactedObject.GetComponent<Regime>().bananasDataScriptableObject.bananaType;
                            var quantity = _interactedObject.GetComponent<Regime>().bananasDataScriptableObject
                                .regimeQuantity;

                            Inventory.Instance.AddQuantity(bananaType, quantity);
                            UIQueuedMessages.Instance.AddMessage("+ "+quantity+" "+bananaType.ToString().ToLower()+" bananas");
                            // TODO change bananier state to baby bananier
                            break;
                        case ItemType.BOSS_FIGHT_LAUNCHER:
                            BossManager.Instance.StartBossFight(BossType.KELSAIK);
                            break;
                    }
                }
            }
        }

        void LeaveInteraction() {
            _interactedObject = null;
            lootMessage.SetActive(false);
        }

        void ShowLootMessage() {
            lootMessage.SetActive(true);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer == _itemsLayerMask.value && GameManager.Instance.isGamePlaying) {
                if (other.gameObject.GetComponent<Item>().itemType == ItemType.BOSS_FIGHT_LAUNCHER) {
                    UIFace.Instance.GetIntrigued();
                }
            }
        }

        private void OnTriggerStay(Collider other) {
            if (other.gameObject.layer == _itemsLayerMask.value && GameManager.Instance.isGamePlaying) {
                if (other.gameObject != _interactedObject) {
                    ShowLootMessage();
                    Interact(other.gameObject);
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            LeaveInteraction();
        }
    }
}