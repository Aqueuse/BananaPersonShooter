using Dialogues;
using Enums;
using Game.CommandRoomPanelControls;
using UI.InGame;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Items {
    public class ItemsManager : MonoBehaviour {
        private GameObject _interactedObject;
        private const int ItemsLayerMask = 1 << 8;

        private ItemStaticType _interactedItemStaticType;
        

        private void Update() {
            if (!ObjectsReference.Instance.gameManager.isGamePlaying || ObjectsReference.Instance.gameManager.gameContext != GameContext.IN_GAME) return;
            
            if (Physics.Raycast(transform.position, transform.forward,  out RaycastHit raycastHit, 10, ItemsLayerMask)) {
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
                    case ItemStaticType.DOOR:
                        ObjectsReference.Instance.audioManager.PlayEffect(EffectType.OPEN_DOOR, 0);
                        
                        ObjectsReference.Instance.scenesSwitch.SwitchScene(_interactedObject.GetComponent<Door>().destinationMap.ToUpper(), _interactedObject.GetComponent<Door>().spawnPoint, false);
                        _interactedObject = null;
                        break;
                    case ItemStaticType.MINI_CHIMP:
                        _interactedObject.GetComponent<SpeechToVoice>().Play();
                        break;
                    case ItemStaticType.UIMAP:
                        var uImap = _interactedObject.GetComponent<UIMap>();
                        ObjectsReference.Instance.scenesSwitch.Teleport(uImap.spawnPoint);
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
                }
            }
        }

        public void HideAllItemsStatics() {
            foreach (var itemStatic in GameObject.FindGameObjectsWithTag("ItemStatic")) {
                itemStatic.GetComponentInParent<ItemStatic>().Desactivate();
            }
        }
    }
}