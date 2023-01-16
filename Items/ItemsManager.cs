using Audio;
using Building;
using Dialogues;
using Enums;
using Monkeys;
using Player;
using UI;
using UI.InGame;
using UnityEngine;

namespace Items {
    public class ItemsManager : MonoSingleton<ItemsManager> {
        private GameObject _interactedObject;
        private int _itemsLayerMask = 1 << 8;

        public GameObject lootMessage;
        
        private void Update() {
            if (GameManager.Instance.isGamePlaying) {
                if (Physics.Raycast(transform.position, transform.forward,  out RaycastHit raycastHit, 10, _itemsLayerMask)) {
                    if (raycastHit.transform.gameObject != _interactedObject) {
                        lootMessage.SetActive(true);
                        _interactedObject = raycastHit.transform.gameObject;
                    }
                }
                else {
                    _interactedObject = null;
                    lootMessage.SetActive(false);
                }
            }
        }

        public void Validate() {
            if (_interactedObject != null) {
                AudioManager.Instance.PlayEffect(EffectType.BUTTON_ITERACTION);
                ItemType itemType = _interactedObject.GetComponent<Item>().itemType;

                switch (itemType) {
                    case ItemType.DOOR:
                        Transform spawnPoint = _interactedObject.GetComponent<Door>().spawnPoint;
                        GameManager.Instance.SwitchScene(_interactedObject.GetComponent<Door>().destinationMap,
                            spawnPoint.position);
                        _interactedObject = null;
                        lootMessage.SetActive(false);
                        break;
                    case ItemType.REGIME:
                        var bananaType = _interactedObject.GetComponent<Regime>().bananasDataScriptableObject.itemThrowableType;
                        var quantity = _interactedObject.GetComponent<Regime>().bananasDataScriptableObject
                            .regimeQuantity;

                        Inventory.Instance.AddQuantity(bananaType, quantity);
                        UIQueuedMessages.Instance.AddMessage("+ "+quantity+" "+bananaType.ToString().ToLower()+" bananas");
                        // TODO change bananier state to baby bananier
                        break;
                    case ItemType.BOSS_FIGHT_LAUNCHER:
                        MonkeyManager.Instance.StartBossFight(MonkeyType.KELSAIK);
                        break;
                    case ItemType.MINI_CHIMP:
                        DialogueSystem.Instance.interact_with_minichimp(_interactedObject);
                        break;
                    case ItemType.MOVER:
                        BananaMan.Instance.advancementType = AdvancementType.OTHER;
                        Mover.Instance.Acquire();
                        Destroy(_interactedObject);
                        break;
                    case ItemType.MINI_CHIMP_BUILD_STATION:
                        if (!BananaMan.Instance.hasMover) Mover.Instance.Acquire();
                        UIManager.Instance.Show_Hide_minichimp_plateform_builder_interface(true);
                        break;
                }
            }
        }
    }
}