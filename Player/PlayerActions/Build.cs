using System;
using Data;
using Gestion;
using Gestion.BuildablesBehaviours;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.PlayerActions {
    public class Build : MonoBehaviour {
        [SerializeField] private LayerMask buildingLayerMask;
        [SerializeField] private Camera mainCamera;
        
        [SerializeField] private GhostsReference ghostsReference;
        
        public GameObject _activeGhost;
        private Ghost _activeGhostClass;

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

        private GameObject _buildable;
        
        private void Update() {
            ray = mainCamera.ScreenPointToRay(Mouse.current.position.value);

            _activeGhost.transform.position = ObjectsReference.Instance.uiHud.buildablePlacementTransform.position;

            float distance = Vector3.Distance(mainCamera.transform.position, _activeGhost.transform.position);
                
            if (Physics.Raycast(ray, out raycastHit, distance, layerMask:buildingLayerMask)) {
                _activeGhost.transform.position = raycastHit.point;
            }
        }

        public void ActivatePlateformGhost() {
            _activeGhost = ghostsReference.GetGhostByBuildableType(BuildableType.PLATEFORM);
            _activeGhostClass = _activeGhost.GetComponent<Ghost>();
            
            if (ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredients(BuildableType.PLATEFORM)) {
                _activeGhostClass.SetGhostState(GhostState.VALID);
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowNormalPlaceHelper();
            }

            ghostRotationEuler = _activeGhost.transform.rotation.eulerAngles;
        }

        public void CancelGhost() {
            if (_activeGhost != null) {
                _activeGhost.transform.position = ghostsReference.transform.position;
                _activeGhost.transform.rotation = Quaternion.identity;
            }
            
            _activeGhost = null;
        }

        public void RotateGhost(Vector3 rotationVector) {
            if (_activeGhost == null) return;

            ghostRotationEuler += rotationVector * 15f;

            ghostRotationEuler.y %= 360;
            ghostRotationEuler.z %= 360;

            _activeGhost.transform.Rotate(rotationVector * 15f, Space.Self);
            
            //ghostRotation = Quaternion.Euler(ghostRotationEuler);
            //_activeGhost.transform.rotation = ghostRotation;
        }

        public void ValidateBuildable() {
            if (_activeGhost == null) return;

            if (_activeGhostClass.GetGhostState() == GhostState.VALID) {
                ObjectsReference.Instance.gameData.currentMapData.isDiscovered = true;
                
                _buildable = Instantiate(original: _activeGhostClass.buildablePropertiesScriptableObject.buildablePrefab,
                    position: _activeGhost.transform.position, rotation: _activeGhost.transform.rotation);

                _buildable.transform.parent = Map.Instance.aspirablesContainer.transform;

                var _craftingIngredients = _activeGhostClass.buildablePropertiesScriptableObject.rawMaterialsWithQuantity;

                foreach (var craftingIngredient in _craftingIngredients) {
                    ObjectsReference.Instance.rawMaterialsInventory.RemoveQuantity(craftingIngredient.Key,
                        craftingIngredient.Value);
                }
                
                _buildable.GetComponent<BuildableBehaviour>().buildableGuid = Guid.NewGuid().ToString();
                
                ObjectsReference.Instance.uiBlueprintsInventory.RefreshUInventory();

                _activeGhost.transform.position = ghostsReference.transform.position;
                _activeGhost = null;
            }
        }

        public void CancelBuild() {
            CancelGhost();
            ObjectsReference.Instance.bananaGun.UngrabBananaGun();
        }

        public void setGhostColor() {
            if (_activeGhost != null)
                if (ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredientsForPlateform()) {
                    _activeGhostClass.SetGhostState(GhostState.VALID);
                }
                else {
                    if (_activeGhostClass.GetGhostState() != GhostState.UNBUILDABLE)
                        _activeGhostClass.SetGhostState(GhostState.NOT_ENOUGH_MATERIALS);
                }
        }
    }
}
