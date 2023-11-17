using System;
using Cameras;
using Enums;
using Input;
using Tags;
using UnityEngine;

namespace Gestion {
    public class GestionMode : MonoBehaviour {
        public GameObject targetedGameObject;
        public bool isGestionModeActivated;
        
        public LayerMask GestionModeSelectableLayerMask;
        
        private Camera mainCamera;
        private Ray ray;

        private UiActions uiActions;

        private Tag targetedGameObjectTag;
        
        private void Start() {
            mainCamera = Camera.main;
            uiActions = ObjectsReference.Instance.uiActions;
        }

        private void Update() {
            if (!isGestionModeActivated) return;

            ray = mainCamera.ScreenPointToRay(uiActions.pointerPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Single.PositiveInfinity, layerMask: GestionModeSelectableLayerMask)) {
                if (targetedGameObject == null) {
                    targetedGameObject = hit.transform.gameObject;
                }
                
                targetedGameObjectTag = targetedGameObject.GetComponent<Tag>();
                
                ObjectsReference.Instance.descriptionsManager.SetDescription(targetedGameObjectTag.itemScriptableObject);
            }

            else {
                targetedGameObject = null;

                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();
            }
        }

        public void SwitchToGestionMode() {
            ObjectsReference.Instance.descriptionsManager.HideAllPanels();
            ObjectsReference.Instance.uiManager.ShowInventories();
            ObjectsReference.Instance.uiBlueprintsInventory.RefreshUInventory();
            
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.BUILDABLES, true);

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0;
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            
            ObjectsReference.Instance.gestionCamera.enabled = true;
            
            ObjectsReference.Instance.mainCamera.Switch_To_Camera_View(CAMERA_MODE.TOP_DOWN_VIEW);

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GESTION);

            isGestionModeActivated = true;
        }
        
        public void CloseGestionMode() {
            ObjectsReference.Instance.build.CancelGhost();
            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();
            
            ObjectsReference.Instance.mainCamera.Switch_To_Camera_View(CAMERA_MODE.PLAYER_VIEW);
            
            ObjectsReference.Instance.uiManager.HideInventories();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.BUILDABLES, false);
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1;
            ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
            
            if (targetedGameObject != null) {
                targetedGameObject.GetComponent<Renderer>().material.color = Color.white;
            }

            isGestionModeActivated = false;
            
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
            
            ObjectsReference.Instance.gestionCamera.enabled = false;
            ObjectsReference.Instance.descriptionsManager.HideAllPanels();
        }
    }
}