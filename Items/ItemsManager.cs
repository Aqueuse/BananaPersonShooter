﻿using Audio;
using Building;
using Dialogues;
using Enums;
using UI;
using UI.InGame;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Items {
    public class ItemsManager : MonoSingleton<ItemsManager> {
        private GameObject _interactedObject;
        private const int ItemsLayerMask = 1 << 8;

        public GameObject lootMessage;
        
        private void Update() {
            if (GameManager.Instance.isGamePlaying) {
                if (Physics.Raycast(transform.position, transform.forward,  out RaycastHit raycastHit, 10, ItemsLayerMask)) {
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
                AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION);
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

                        Inventory.Instance.AddQuantity(bananaType, ItemThrowableCategory.BANANA, quantity);
                        UIQueuedMessages.Instance.AddMessage(
                            "+ "+
                            quantity+" "+
                            LocalizationSettings.StringDatabase.GetTable("bananes").GetEntry(bananaType.ToString().ToLower()).GetLocalizedString());
                        
                        _interactedObject.GetComponent<Regime>().GrabBananas();
                        break;
                    case ItemType.BOSS_FIGHT_LAUNCHER:
                        MapManager.Instance.StartBossFight(MonkeyType.KELSAIK);
                        break;
                    case ItemType.MINI_CHIMP:
                        DialogueSystem.Instance.interact_with_minichimp(_interactedObject);
                        break;
                    case ItemType.MINI_CHIMP_BUILD_STATION:
                        UIManager.Instance.Show_Hide_minichimp_plateform_builder_interface(true);
                        break;
                    case ItemType.FOUNDRY:
                        Foundry.Instance.Load_One_More_Debris();
                        break;
                    case ItemType.INGOT_FOUNDRY_BOX:
                        Foundry.Instance.Give_Ingots_To_Player();
                        break;
                }
            }
        }
    }
}