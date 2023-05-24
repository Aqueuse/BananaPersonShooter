using System.Collections.Generic;
using Building.Buildables;
using Data;
using Enums;
using Items;
using UnityEngine;

namespace Building {
    public class BananaGunGet : MonoBehaviour {
        [SerializeField] private GameObject moverTarget;
        [SerializeField] private LayerMask aspirableLayerMask;

        private GameObject _targetedGameObject;
        
        private ItemType _targetType;

        private bool _isAspiring;

        private DissolveMe dissolveClass;

        private Dictionary<AdvancementState, BuildableType[]> _buildableUnlockedByAdvancementState;

        private ScriptableObjectManager _scriptableObjectManager;

        private void Start() {
            _scriptableObjectManager = ObjectsReference.Instance.scriptableObjectManager;

            _buildableUnlockedByAdvancementState = new Dictionary<AdvancementState, BuildableType[]> {
                { AdvancementState.GRAB_DEBRIS_ON_MAP, new [] { BuildableType.PLATEFORM , BuildableType.FIRST_DOOR_LEFT, BuildableType.FIRST_DOOR_RIGHT, BuildableType.FIRST_CLOISON } },
                { AdvancementState.GRAB_BANANAS, new [] { BuildableType.BANANA_DRYER } }
            };
        }

        private void Update() {
            if (!_isAspiring || !ObjectsReference.Instance.bananaMan.isGrabingBananaGun) return;
            
            ObjectsReference.Instance.bananaGun.transform.LookAt(moverTarget.transform, Vector3.up);

            if (Physics.Raycast(ObjectsReference.Instance.gameManager.cameraMain.transform.position, ObjectsReference.Instance.gameManager.cameraMain.transform.forward, out RaycastHit raycastHit, 100, layerMask:aspirableLayerMask)) {
                _targetedGameObject = raycastHit.transform.gameObject;

                var targetedGameObjectMesh = _targetedGameObject.GetComponent<MeshFilter>().sharedMesh; 

                // Regime
                if (targetedGameObjectMesh == _scriptableObjectManager._meshReferenceScriptableObject.matureBananaTree) {
                    ObjectsReference.Instance.audioManager.PlayEffect(EffectType.GRAB_BANANAS, 0);

                    var regimeClass = _targetedGameObject.GetComponent<Regime>(); 

                    var bananaType = regimeClass.bananasDataScriptableObject.itemType;
                    var quantity = regimeClass.bananasDataScriptableObject.regimeQuantity;

                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.BANANA, bananaType, quantity);
                    
                    regimeClass.GrabBananas();
                    
                    TryAddBlueprintByAdvancementState(AdvancementState.GRAB_BANANAS);
                }

                else {
                    if (_targetedGameObject.GetComponent<DissolveMe>() == null) {
                        // Buildable
                        if (_scriptableObjectManager.IsBuildable(targetedGameObjectMesh)) {
                            dissolveClass = _targetedGameObject.AddComponent<DissolveMe>();
                            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.DESINTEGRATION, 0);
                            dissolveClass.Dissolve(ItemCategory.BUILDABLE,
                                _scriptableObjectManager.GetBuildableTypeByMesh(targetedGameObjectMesh), ItemType.EMPTY);
                        }

                        else {
                            // Debris
                            
                            if (_scriptableObjectManager.IsDebris(targetedGameObjectMesh)) {
                                _targetedGameObject = raycastHit.transform.gameObject;
                                MapItems.Instance.uiCanvasItemsHiddableManager.RemoveCanva(_targetedGameObject
                                    .GetComponentInChildren<Canvas>());

                                TryAddBlueprintByAdvancementState(AdvancementState.GRAB_DEBRIS_ON_MAP);

                                dissolveClass = _targetedGameObject.AddComponent<DissolveMe>();
                                ObjectsReference.Instance.audioManager.PlayEffect(EffectType.DESINTEGRATION, 0);
                                dissolveClass.Dissolve(ItemCategory.RAW_MATERIAL, BuildableType.EMPTY, ItemType.DEBRIS);
                            }
                        }
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
        }

        private void TryAddBlueprintByAdvancementState(AdvancementState advancementState) {
            if (!ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(advancementState)) {
                ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Add(advancementState);
                foreach (var buildableType in _buildableUnlockedByAdvancementState[advancementState]) {
                    ObjectsReference.Instance.uiBlueprints.SetVisible(buildableType);
                }
            }
        }
    }
}