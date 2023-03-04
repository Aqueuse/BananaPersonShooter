using Audio;
using Building;
using Building.Plateforms;
using Dialogues;
using Enums;
using Game;
using UI.InGame;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Items {
    public class ItemsManager : MonoSingleton<ItemsManager> {
        private GameObject _interactedObject;
        private const int ItemsLayerMask = 1 << 8;
        
        private void Update() {
            if (GameManager.Instance.isGamePlaying && GameManager.Instance.gameContext == GameContext.IN_GAME) {
                if (Physics.Raycast(transform.position, transform.forward,  out RaycastHit raycastHit, 10, ItemsLayerMask)) {
                    _interactedObject = raycastHit.transform.gameObject;
                    UIinGameManager.Instance.HideAllUIsinGame();
                    _interactedObject.GetComponent<UICanvasItemsStatic>().ShowUI();
                }
                else {
                    if (_interactedObject != null) {
                        UIinGameManager.Instance.HideAllUIsinGame();
                        _interactedObject = null;
                    }
                }
            }
        }


        public void Validate() {
            if (_interactedObject != null) {
                ItemStaticType itemStaticType = _interactedObject.GetComponent<ItemStatic>().itemStaticType;

                switch (itemStaticType) {
                    case ItemStaticType.DOOR:
                        AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION);
                        
                        Transform spawnPoint = _interactedObject.GetComponent<Door>().spawnPoint;
                        ScenesSwitch.Instance.SwitchScene(_interactedObject.GetComponent<Door>().destinationMap.ToUpper(), spawnPoint.position, false);
                        _interactedObject = null;
                        break;
                    case ItemStaticType.REGIME:
                        AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION);

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
                    case ItemStaticType.MINI_CHIMP:
                        DialoguesManager.Instance.SetActiveMiniChimp(_interactedObject);
                        SimpleInteraction.Instance.StartSimpleInteraction();
                        break;
                    case ItemStaticType.BUILD_STATION:
                        BuildStation.Instance.ShowBuildStationInterface();
                        break;
                    case ItemStaticType.FOUNDRY:
                        Foundry.Instance.Load_One_More_Debris();
                        break;
                    case ItemStaticType.INGOT_FOUNDRY_BOX:
                        Foundry.Instance.Give_Ingots_To_Player();
                        break;
                    case ItemStaticType.GRABBABLE:
                        _interactedObject.GetComponent<GrabbableItem>().GrabPrintedItem();
                        break;
                }
            }
        }
    }
}