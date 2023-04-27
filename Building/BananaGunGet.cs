using System.Collections.Generic;
using Building.Buildables;
using Enums;
using Items;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Building {
    public class BananaGunGet : MonoBehaviour {
        [SerializeField] private GameObject moverTarget;
        private GameObject _targetedGameObject;
        
        private ItemType _targetType;

        private bool _isAspiring;

        private Dictionary<AdvancementState, BuildableType> buildableUnlockedByAdvancementState;

        private void Start() {
            buildableUnlockedByAdvancementState = new Dictionary<AdvancementState, BuildableType> {
                { AdvancementState.GRAB_DEBRIS_ON_MAP, BuildableType.PLATEFORM },
                { AdvancementState.GRAB_BANANAS, BuildableType.BANANA_DRYER }
            };
        }

        private void Update() {
            if (!_isAspiring || !ObjectsReference.Instance.bananaMan.isGrabingBananaGun) return;
            
            ObjectsReference.Instance.bananaGun.transform.LookAt(moverTarget.transform, Vector3.up);

            if (Physics.Raycast(ObjectsReference.Instance.gameManager.cameraMain.transform.position, ObjectsReference.Instance.gameManager.cameraMain.transform.forward, out RaycastHit raycastHit, 100)) {
                if (!raycastHit.transform.gameObject.CompareTag("Aspirables")) return;

                if (raycastHit.transform.GetComponent<ItemThrowable>() != null) {
                    ItemThrowable itemThrowable = raycastHit.transform.GetComponent<ItemThrowable>();
                    _targetedGameObject = raycastHit.transform.gameObject;

                    if (itemThrowable.itemCategory == ItemCategory.BUILDABLE) {
                        ObjectsReference.Instance.audioManager.PlayEffect(EffectType.DESINTEGRATION, 0);
                        _targetedGameObject.GetComponent<Buildable>().DissolveMe();
                    }

                    if (itemThrowable.itemCategory == ItemCategory.REGIME) {
                        ObjectsReference.Instance.audioManager.PlayEffect(EffectType.GRAB_BANANAS, 0);

                        var bananaType = _targetedGameObject.GetComponent<Regime>().bananasDataScriptableObject.itemType;
                        var quantity = _targetedGameObject.GetComponent<Regime>().bananasDataScriptableObject.regimeQuantity;

                        ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.BANANA, bananaType, quantity);
                        
                        _targetedGameObject.GetComponent<Regime>().GrabBananas();
                        
                        TryAddBlueprintByAdvancementState(AdvancementState.GRAB_BANANAS);
                    }

                    if (itemThrowable.itemType == ItemType.DEBRIS) {
                        _targetedGameObject = raycastHit.transform.gameObject;
                        var debrisClass = _targetedGameObject.GetComponent<Debris>(); 
                        MapItems.Instance.uiCanvasItemsHiddableManager.RemoveCanva(_targetedGameObject.GetComponentInChildren<Canvas>());

                        ObjectsReference.Instance.audioManager.PlayEffect(EffectType.DESINTEGRATION, 0);
                        
                        debrisClass.DissolveMe();

                        TryAddBlueprintByAdvancementState(AdvancementState.GRAB_DEBRIS_ON_MAP);
                    }
                }
            }
        }

        public void StartToGet() {
            _isAspiring = true;
            ObjectsReference.Instance.bananaGun.GrabBananaGun();
            ObjectsReference.Instance.uiCrosshair.SetCrosshair(ItemCategory.BUILDABLE, ItemType.EMPTY);
        }

        public void CancelGet() {
            _isAspiring = false;
            
            ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.EFFECT);

            ObjectsReference.Instance.bananaGun.CancelMover();
        }

        private void TryAddBlueprintByAdvancementState(AdvancementState advancementState) {
            if (!ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(advancementState)) {
                ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Add(advancementState);
                ObjectsReference.Instance.uiBlueprints.SetVisible(buildableUnlockedByAdvancementState[advancementState]);
            }
        }
    }
}