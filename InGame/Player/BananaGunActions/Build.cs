using System;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties;
using Tags;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.Player.BananaGunActions {
    public class Build : MonoBehaviour {
        public GameObject targetedGameObject;

        [SerializeField] private LayerMask GestionViewSelectableLayerMask;

        private Mesh _targetedGameObjectMesh;
        private BananaType _targetType;

        private GenericDictionary<ItemScriptableObject, int> rawMaterialsWithQuantity;
        private GenericDictionary<ItemScriptableObject, int> craftingMaterials;
        
        private Regime regimeClass;
        private Tag gameObjectTagClass;
        private GAME_OBJECT_TAG gameObjectTag;

        [SerializeField] private LayerMask buildingLayerMask;
        [SerializeField] private Camera mainCamera;
        
        [SerializeField] private GhostsReference ghostsReference;

        public bool isBuilding;
        public GameObject _activeGhost;
        private Ghost _activeGhostClass;
        public BuildableType buildableType;

        public Transform buildablePlacementTransform;

        private Ray ray;
        private RaycastHit raycastHit;
        
        private GameObject _buildable;
        
        private void OnEnable() {
            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowBuildHelper();

            SetActiveBuildable(ObjectsReference.Instance.bananaMan.bananaManData.activeBuildable);
            ActivateGhost();
        }

        private void OnDisable() {
            HideGhost();
        }

        private void Update() {
            ray = mainCamera.ScreenPointToRay(Mouse.current.position.value);

            _activeGhost.transform.position = ObjectsReference.Instance.build.buildablePlacementTransform.position;

            var distance = Vector3.Distance(mainCamera.transform.position, _activeGhost.transform.position);
                
            if (Physics.Raycast(ray, out raycastHit, distance, layerMask:buildingLayerMask)) {
                _activeGhost.transform.position = raycastHit.point;
            }

            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit, 2000, layerMask: GestionViewSelectableLayerMask)) {
                targetedGameObject = raycastHit.transform.gameObject;
                
                ObjectsReference.Instance.uiCrosshairs.SetCrosshair(CrosshairType.DESTROY_BUILD);
            }
            else {
                targetedGameObject = null;
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();
            }
        }

        public void SetActiveBuildable(BuildableType buildableType) {
            this.buildableType = buildableType;
        }

        private void ActivateGhost() {
            _activeGhost = ghostsReference.GetGhostByBuildableType(buildableType);
            _activeGhostClass = _activeGhost.GetComponent<Ghost>();
            
            if (ObjectsReference.Instance.bananaManRawMaterialInventory.HasCraftingIngredients(_activeGhostClass.buildablePropertiesScriptableObject)) {
                _activeGhostClass.SetGhostState(GhostState.VALID);
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowNormalPlaceHelper();
                ObjectsReference.Instance.uiFlippers.SetBuildablePlacementAvailability(true, _activeGhostClass.buildablePropertiesScriptableObject);
            }

            else {
                _activeGhostClass.SetGhostState(GhostState.NOT_ENOUGH_MATERIALS);
                ObjectsReference.Instance.uiFlippers.SetBuildablePlacementAvailability(false, _activeGhostClass.buildablePropertiesScriptableObject);
            }
        }

        public void HideGhost() {
            if (_activeGhost == null) return;
            
            _activeGhost.transform.position = ghostsReference.transform.position;
            _activeGhost.transform.rotation = Quaternion.identity;
        }

        public void RotateGhost(Vector3 rotationVector) {
            _activeGhost.transform.RotateAround(_activeGhost.transform.position, rotationVector, 30f);
        }

        public void ValidateBuildable() {
            if (_activeGhostClass.GetGhostState() == GhostState.VALID) {
                _buildable = Instantiate(
                    _activeGhostClass.buildablePropertiesScriptableObject.buildablePrefab,
                    _activeGhost.transform.position, 
                    _activeGhost.transform.rotation,
                    ObjectsReference.Instance.gameSave.savablesItemsContainer
                );

                var _craftingIngredients = _activeGhostClass.buildablePropertiesScriptableObject.rawMaterialsWithQuantity;

                foreach (var craftingIngredient in _craftingIngredients) {
                    ObjectsReference.Instance.bananaManRawMaterialInventory.RemoveQuantity(
                        craftingIngredient.Key,
                        craftingIngredient.Value
                    );
                    
                    ObjectsReference.Instance.uiQueuedMessages.RemoveFromInventory(
                        craftingIngredient.Key, 
                        craftingIngredient.Value
                    );
                    ObjectsReference.Instance.uiFlippers.RefreshActiveBuildableAvailability();
                }

                _buildable.GetComponent<BuildableBehaviour>().buildableGuid = Guid.NewGuid().ToString();
            }
        }
        
        public void RepairOrHarvest() {
            if (targetedGameObject == null) return;

            gameObjectTagClass = targetedGameObject.GetComponent<Tag>();
            gameObjectTag = gameObjectTagClass.gameObjectTag;
            
            if (gameObjectTag == GAME_OBJECT_TAG.BUILDABLE) {
                if (targetedGameObject.TryGetComponent(out BuildableBehaviour buildableBehaviour)) {
                    if (buildableBehaviour.isBreaked) {
                        TryToRepairBuildable(buildableBehaviour);
                    }
                    
                    else {
                        harvest(gameObjectTagClass.itemScriptableObject);
                    }
                }
            }

            else {
                harvest(gameObjectTagClass.itemScriptableObject);
            }
        }

        public void harvest(ItemScriptableObject itemScriptableObject) {
            if (itemScriptableObject.itemCategory == ItemCategory.BUILDABLE) {
                targetedGameObject.GetComponent<BuildableBehaviour>().RetrieveRawMaterials();
                
                DestroyImmediate(targetedGameObject);
                targetedGameObject = null;
                
                ObjectsReference.Instance.uiCrosshairs.SetCrosshair(CrosshairType.DEFAULT);

                ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);

                ObjectsReference.Instance.uiFlippers.RefreshActiveBuildableAvailability();
                ObjectsReference.Instance.uiFlippers.RefreshActiveDroppableQuantity();

                ObjectsReference.Instance.build.setGhostColor();
            }
        }
        
        private void TryToRepairBuildable(BuildableBehaviour buildableBehaviour) {
            if (!ObjectsReference.Instance.bananaManRawMaterialInventory.HasCraftingIngredients(buildableBehaviour.buildablePropertiesScriptableObject)) return;
            
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);
            
            buildableBehaviour.RepairBuildable();
        }
        
        private void setGhostColor() {
            if (_activeGhost != null)
                _activeGhostClass.SetGhostState(
                    ObjectsReference.Instance.bananaManRawMaterialInventory.HasCraftingIngredients(_activeGhostClass
                        .buildablePropertiesScriptableObject)
                        ? GhostState.VALID
                        : GhostState.NOT_ENOUGH_MATERIALS);
        }
    }
}
