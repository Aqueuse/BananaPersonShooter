using Building.Buildables.DoorLeft;
using Building.Buildables.DoorRight;
using Dialogues;
using Enums;
using Game.BananaCannonMiniGame;
using Game.CommandRoomPanelControls;
using Game.Steam;
using UnityEngine;

namespace Items {
    public class ItemsManager : MonoBehaviour {
        [SerializeField] private LayerMask itemsLayerMask;
        [SerializeField] private Transform grabbableTarget;
        
        public bool isGrabbing;
        private Vector3 grabbablePosition;
        private Rigidbody grabbableRigidbody;
        
        private GameObject _interactedObject;
        private ItemStaticType _interactedItemStaticType;
        
        private void Update() {
            if (!ObjectsReference.Instance.gameManager.isGamePlaying || ObjectsReference.Instance.gameManager.gameContext != GameContext.IN_GAME || isGrabbing) return;

            if (Physics.Raycast(transform.position, transform.forward,  out var raycastHit, 10, itemsLayerMask)) {
                if (_interactedObject != null) _interactedObject.GetComponent<ItemStatic>().Desactivate(); // magic ( ͡• ͜ʖ ͡• )
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

        private void FixedUpdate() {
            if (isGrabbing) {
                grabbableRigidbody.velocity = (grabbableTarget.position - _interactedObject.transform.position) * 10;
            }
        }

        public void Validate() {
            if (_interactedObject != null && !isGrabbing) {
                _interactedItemStaticType = _interactedObject.GetComponent<ItemStatic>().itemStaticType;

                switch (_interactedItemStaticType) {
                    case ItemStaticType.DOOR_BEETWEEN_LEVELS:
                        ObjectsReference.Instance.audioManager.PlayEffect(EffectType.OPEN_DOOR, 0);

                        ObjectsReference.Instance.scenesSwitch.SwitchScene(_interactedObject.GetComponent<Door>().destinationMap.ToUpper(), _interactedObject.GetComponent<Door>().spawnPoint, false, false);
                        _interactedObject = null;
                        break;
                    case ItemStaticType.MINI_CHIMP:
                        _interactedObject.GetComponent<SpeechToVoice>().Play();
                        break;
                    case ItemStaticType.BANANAGUN:
                        ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.HUD, true);
                        ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(true);
                        // TODO : animation take banana gun
                        
                        ObjectsReference.Instance.audioManager.PlayEffect(EffectType.GUITAR_RYTHM_01, 0);
                        ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Add(AdvancementState.GET_BANANAGUN);
                        ObjectsReference.Instance.steamIntegration.UnlockAchievement(SteamAchievement.STEAM_ACHIEVEMENT_BANAGUN_RECONSTRUCTED);
                        
                        _interactedObject.SetActive(false);
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

        public void Grab() {
            if (_interactedObject != null) {
                if (_interactedObject.GetComponent<ItemStatic>().itemStaticType == ItemStaticType.GRABBABLE_PIECE) {
                    grabbableRigidbody = _interactedObject.GetComponent<Rigidbody>();
                    grabbableRigidbody.useGravity = false;
                    
                    _interactedObject.GetComponent<ItemStatic>().Desactivate();

                    isGrabbing = true;
                    ObjectsReference.Instance.audioManager.PlayEffect(EffectType.GRAB_SOMETHING, 0);
                }
            }
        }

        public void Release() {
            // let the object fall
            if (_interactedObject != null) {
                if (_interactedObject.GetComponent<ItemStatic>().itemStaticType == ItemStaticType.GRABBABLE_PIECE) {
                    grabbableRigidbody = _interactedObject.GetComponent<Rigidbody>();
                    grabbableRigidbody.useGravity = true;
                    grabbableRigidbody.velocity = Vector3.zero;

                    _interactedObject.GetComponent<ItemStatic>().Desactivate();
                    _interactedObject = null;
                    isGrabbing = false;
                }
            }
        }
    } 
}