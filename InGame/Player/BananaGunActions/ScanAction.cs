using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsData.Buildables;
using Tags;
using UnityEngine;

namespace InGame.Player.BananaGunActions {
    public class ScanAction : MonoBehaviour {
        public GameObject targetedGameObject;

        [SerializeField] private LayerMask GestionViewSelectableLayerMask;

        private Mesh _targetedGameObjectMesh;
        private BananaType _targetType;

        [SerializeField] private Camera mainCamera;
        
        private Tag gameObjectTagClass;
        
        private RaycastHit raycastHit;

        private void Update() {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit, 2000, layerMask: GestionViewSelectableLayerMask)) {
                targetedGameObject = raycastHit.transform.gameObject;

                var targetedGameObjectTag = targetedGameObject.GetComponent<Tag>().gameObjectTag;

                switch (targetedGameObjectTag) {
                    case GAME_OBJECT_TAG.BUILDABLE:
                        if (targetedGameObject.GetComponent<BuildableData>().buildableState == BuildableState.BLUEPRINT) { 
                            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MISSING_MATERIALS_PANEL, true);
                            ObjectsReference.Instance.uiMissingMaterialBuildingPanel.ShowMissingMaterials(targetedGameObject.GetComponent<BuildableBehaviour>().GetMissingMaterialsWithQuantity());
                        }
                        break;
                }
            }
            else {
                targetedGameObject = null;
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MISSING_MATERIALS_PANEL, false);
            }
        }
    }
}