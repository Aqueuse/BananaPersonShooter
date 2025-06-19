using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.Player.BananaGunActions {
    public class BuildAction : MonoBehaviour {
        private GenericDictionary<ItemScriptableObject, int> rawMaterialsWithQuantity;
        private GenericDictionary<ItemScriptableObject, int> craftingMaterials;
        
        private Regime regimeClass;

        [SerializeField] private LayerMask buildingLayerMask;
        [SerializeField] private Camera mainCamera;
        
        [SerializeField] private GhostsReference ghostsReference;

        public GameObject _activeGhost;
        private Ghost _activeGhostClass;
        public BuildableType buildableType;

        public Transform buildablePlacementTransform;

        private Ray ray;
        private RaycastHit raycastHit;
        
        private void OnEnable() {
            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowBuildHelper();
            ActivateGhost();
        }

        private void OnDisable() {
            HideGhost();
        }

        private void Update() {
            ray = mainCamera.ScreenPointToRay(Mouse.current.position.value);

            _activeGhost.transform.position = ObjectsReference.Instance.buildAction.buildablePlacementTransform.position;

            var distance = Vector3.Distance(mainCamera.transform.position, _activeGhost.transform.position);
                
            if (Physics.Raycast(ray, out raycastHit, distance, layerMask:buildingLayerMask)) {
                _activeGhost.transform.position = raycastHit.point;
            }
        }

        public void SetActiveBuildable(BuildableType buildableType) {
            this.buildableType = buildableType;
            ActivateGhost();
        }

        private void ActivateGhost() {
            _activeGhost = ghostsReference.GetGhostByBuildableType(buildableType);
            _activeGhostClass = _activeGhost.GetComponent<Ghost>();
            
            if (ObjectsReference.Instance.bananaManRawMaterialInventory.HasCraftingIngredients(_activeGhostClass.buildablePropertiesScriptableObject)) {
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowNormalPlaceHelper();
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

        public void PlaceBlueprint() {
            var buildable = Instantiate(
                _activeGhostClass.buildablePropertiesScriptableObject.buildablePrefab,
                _activeGhost.transform.position, 
                _activeGhost.transform.rotation,
                ObjectsReference.Instance.gameSave.savablesItemsContainer
            );
                
            buildable.GetComponent<BuildableBehaviour>().Init();
        }
    }
}
