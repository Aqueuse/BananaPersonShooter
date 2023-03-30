using Audio;
using Building;
using Building.Plateforms;
using Data;
using Dialogues;
using Enums;
using Game;
using Game.CommandRoomPanelControls;
using Input;
using Input.UIActions;
using Save;
using UI;
using UI.InGame;
using UI.Tutorials;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Items {
    public class ItemsManager : MonoSingleton<ItemsManager> {
        private GameObject _interactedObject;
        private const int ItemsLayerMask = 1 << 8;

        private ItemStaticType _interactedItemStaticType;
        public ItemsDataScriptableObject itemsDataScriptableObject;
        
        private void Update() {
            if (!GameManager.Instance.isGamePlaying || GameManager.Instance.gameContext != GameContext.IN_GAME) return;
            
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
                        AudioManager.Instance.PlayEffect(EffectType.OPEN_DOOR, 0);
                        
                        ScenesSwitch.Instance.SwitchScene(_interactedObject.GetComponent<Door>().destinationMap.ToUpper(), _interactedObject.GetComponent<Door>().spawnPoint, false);
                        _interactedObject = null;
                        break;
                    case ItemStaticType.REGIME:
                        AudioManager.Instance.PlayEffect(EffectType.GRAB_BANANAS, 0);

                        var bananaType = _interactedObject.GetComponent<Regime>().bananasDataScriptableObject.itemThrowableType;
                        var quantity = _interactedObject.GetComponent<Regime>().bananasDataScriptableObject.regimeQuantity;

                        Inventory.Instance.AddQuantity(bananaType, quantity);
                        UIQueuedMessages.Instance.AddMessage(
                            "+ "+
                            quantity+" "+
                            LocalizationSettings.StringDatabase.GetTable("bananes").GetEntry(bananaType.ToString().ToLower()).GetLocalizedString());
                        
                        _interactedObject.GetComponent<Regime>().GrabBananas();
                        break;
                    case ItemStaticType.MINI_CHIMP:
                        _interactedObject.GetComponent<SpeechToVoice>().Play();
                        break;
                    case ItemStaticType.BUILD_STATION:
                        UIBuildStationActions.Instance.activeBuildStation = _interactedObject.GetComponent<BuildStation>();
                        UIManager.Instance.uiBuildStation.ShowBuildStationInterface();
                        break;
                    case ItemStaticType.FOUNDRY:
                        _interactedObject.GetComponentInParent<Foundry>().Load_One_More_Debris();
                        break;
                    case ItemStaticType.INGOT_FOUNDRY_BOX:
                        _interactedObject.GetComponentInParent<Foundry>().Give_Ingots_To_Player();
                        break;
                    case ItemStaticType.GRABBABLE:
                        AudioManager.Instance.PlayEffect(EffectType.GRAB_BANANAS, 0);
                        _interactedObject.GetComponent<GrabbableItem>().GrabPrintedItem();
                        break;
                    case ItemStaticType.UIMAP:
                        var uImap = _interactedObject.GetComponent<UIMap>();
                        ScenesSwitch.Instance.Teleport(uImap.spawnPoint);
                        break;
                    case ItemStaticType.BANANAGUN:
                        AudioManager.Instance.PlayEffect(EffectType.GRAB_BANANAS, 0);
                        UIManager.Instance.Set_active(UICanvasGroupType.HUD, true);
                        BananaGun.Instance.bananaGunInBack.SetActive(true);
                        // TODO : animation take banana gun
                        GameData.Instance.bananaManSavedData.advancementState = AdvancementState.GET_BANANAGUN;
                        InputManager.Instance.uiSchemaContext = UISchemaSwitchType.TUTORIAL;
                        GameManager.Instance.PauseGame(true);
                        TutorialsManager.Instance.Show_Help();
                        CommandRoomControlPanelsManager.Instance.SetBananaGunVisibility(false);
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