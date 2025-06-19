using System;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.Gestion {
    public class GestionViewMode : MonoBehaviour {
        [SerializeField] private LayerMask buildingLayerMask;
        [SerializeField] private LayerMask GestionViewSelectableLayerMask;

        private Camera mainCamera;

        public ViewModeContextType viewModeContextType;
        
        public GameObject targetedGameObject;

        public GameObject _activeGhost;
        private Ghost _activeGhostClass;
        
        private GenericDictionary<RawMaterialType, int> rawMaterialsWithQuantity;

        private Ray ray;
        private RaycastHit raycastHit;
        
        private Quaternion _normalRotation;
        private Vector3 _ghostPosition;
        private Vector3 customRotation;
        private Vector3 ghostRotationEuler;
        private Quaternion ghostRotation;

        private Transform pivotTransform;
        private Vector3 pivotTransformPosition;
        private Quaternion pivotTransformRotation;
        
        private Vector3 pivotLocalePosition;
        private Quaternion pivotLocaleRotation;
        private Vector3 raycastHitPointLocalePosition;
        private float deltaZ;
        private float deltaY;

        private Vector3 offsettedPosition;
        private Vector3 raycastHitPoint;
        
        private void Start() {
            mainCamera = Camera.main;
        }

        private void Update() {
            ray = mainCamera.ScreenPointToRay(Mouse.current.position.value);
            
            targetedGameObject = 
                Physics.Raycast(ray, out raycastHit, Single.PositiveInfinity, layerMask: GestionViewSelectableLayerMask) ? 
                    raycastHit.transform.gameObject : 
                    null;

            if (_activeGhost == null) return;
            
            if(Physics.Raycast(ray, out raycastHit, 2000, layerMask:buildingLayerMask)) {
                _activeGhost.transform.position = raycastHit.point;
            }
        }

        public void StartHarvesting() {
            if (_activeGhost != null) CancelGhost();

            viewModeContextType = ViewModeContextType.HARVEST;
        }

        public void StartRepairing() {
            if (_activeGhost != null) CancelGhost();

            viewModeContextType = ViewModeContextType.REPAIR;
        }

        public void ActivateGhostByScriptableObject(BuildablePropertiesScriptableObject buildablePropertiesScriptableObject) {
            if (_activeGhost != null) CancelGhost();
            
            _activeGhost = ObjectsReference.Instance.ghostsReference.GetGhostByBuildableType(buildablePropertiesScriptableObject.buildableType);
            _activeGhostClass = _activeGhost.GetComponent<Ghost>();
            
            viewModeContextType = ViewModeContextType.BUILD;
            
            if (ObjectsReference.Instance.bananaManRawMaterialInventory.HasCraftingIngredients(buildablePropertiesScriptableObject)) {
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowNormalPlaceHelper();
            }
            
            ghostRotationEuler = _activeGhost.transform.rotation.eulerAngles;
        }
        
        public void CancelGhost() {
            if (_activeGhost != null) _activeGhost.transform.position = ObjectsReference.Instance.ghostsReference.transform.position;
            
            _activeGhost = null;
        }

        public void RotateGhost(Vector3 rotationVector) {
            if (_activeGhost == null) return;

            ghostRotationEuler += rotationVector * 15f;

            ghostRotationEuler.y %= 360;
            ghostRotationEuler.z %= 360;

            ghostRotation = Quaternion.Euler(ghostRotationEuler);
            _activeGhost.transform.rotation = ghostRotation;
        }

        public void ValidateBuildable() {
            if (_activeGhost == null) return;
            
            Instantiate(
                _activeGhostClass.buildablePropertiesScriptableObject.buildablePrefab,
                _activeGhost.transform.position, rotation: _activeGhost.transform.rotation,
                ObjectsReference.Instance.gameSave.savablesItemsContainer
            );

            var _craftingIngredients = _activeGhostClass.buildablePropertiesScriptableObject.rawMaterialsWithQuantity;

            foreach (var craftingIngredient in _craftingIngredients) {
                ObjectsReference.Instance.bananaManRawMaterialInventory.RemoveQuantity(craftingIngredient.Key,
                    craftingIngredient.Value);
                    
                ObjectsReference.Instance.bottomSlots.RefreshSlotsQuantities();

            }
            
            _activeGhost.transform.position = ObjectsReference.Instance.ghostsReference.transform.position;
            _activeGhost = null;
                
            viewModeContextType = ViewModeContextType.SCAN;
        }
        
        public void harvest() {
            if (targetedGameObject == null) return;
        }
    }
}
