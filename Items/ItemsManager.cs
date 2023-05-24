﻿using Building.Buildables.DoorLeft;
using Building.Buildables.DoorRight;
using Dialogues;
using Enums;
using Game.BananaCannonMiniGame;
using Game.CommandRoomPanelControls;
using UnityEngine;

namespace Items {
    public class ItemsManager : MonoBehaviour {
        private GameObject _interactedObject;
        [SerializeField] private LayerMask itemsLayerMask;

        private ItemStaticType _interactedItemStaticType;

        private void Update() {
            if (!ObjectsReference.Instance.gameManager.isGamePlaying || ObjectsReference.Instance.gameManager.gameContext != GameContext.IN_GAME) return;

            if (Physics.Raycast(transform.position, transform.forward,  out RaycastHit raycastHit, 10, itemsLayerMask)) {
                _interactedObject = raycastHit.transform.gameObject;

                _interactedObject.GetComponent<ItemStatic>().Activate();
            }
            else {
                if (_interactedObject != null) {
                    _interactedObject.GetComponent<ItemStatic>().Desactivate();

                    _interactedObject = null;
                }
            }
        }

        public void Validate() {
            if (_interactedObject != null) {
                ItemStaticType itemStaticType = _interactedObject.GetComponent<ItemStatic>().itemStaticType;

                switch (itemStaticType) {
                    case ItemStaticType.DOOR_BEETWEEN_LEVELS:
                        ObjectsReference.Instance.audioManager.PlayEffect(EffectType.OPEN_DOOR, 0);

                        ObjectsReference.Instance.scenesSwitch.SwitchScene(_interactedObject.GetComponent<Door>().destinationMap.ToUpper(), _interactedObject.GetComponent<Door>().spawnPoint, false);
                        _interactedObject = null;
                        break;
                    case ItemStaticType.MINI_CHIMP:
                        _interactedObject.GetComponent<SpeechToVoice>().Play();
                        break;
                    case ItemStaticType.BANANAGUN:
                        ObjectsReference.Instance.audioManager.PlayEffect(EffectType.GRAB_BANANAS, 0);
                        ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.HUD, true);
                        ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(true);
                        // TODO : animation take banana gun
                        ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Add(AdvancementState.GET_BANANAGUN);
                        CommandRoomControlPanelsManager.Instance.SetBananaGunVisibility(false);
                        ObjectsReference.Instance.audioManager.PlayEffect(EffectType.GRAB_BANANAS, 0);
                        break;
                    case ItemStaticType.COMMAND_ROOM_PANEL:
                        CommandRoomControlPanelsManager.Instance.ShowHidePanel(_interactedObject.GetComponent<CommandRoomPanel>().commandRoomPanelType);
                        break;
                    case ItemStaticType.BANANA_CANNON_MINI_GAME:
                        if (_interactedObject.GetComponent<CanvasGroup>().alpha == 0) break;
                        BananaCannonMiniGameManager.Instance.SwitchToMiniGame();
                        break;
                    case ItemStaticType.DOOR_LEFT:
                        _interactedObject.GetComponent<RotationDoorLeft>().Action();
                        break;
                    case ItemStaticType.DOOR_RIGHT:
                        _interactedObject.GetComponent<RotationDoorRight>().Action();
                        break;
                }
            }
        }
    }
}