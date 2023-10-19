using System;
using Cameras;
using Enums;
using Tags;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Gestion {
    public class GestionMode : MonoBehaviour {
        public GameObject targetedGameObject;
        public bool isGestionModeActivated;
        
        public LayerMask GestionModeSelectableLayerMask;
        
        private UniversalAdditionalCameraData URP_Asset;
        private Camera mainCamera;
        private Ray ray;
       
        private void Start() {
            mainCamera = Camera.main;
            URP_Asset = GetComponent<UniversalAdditionalCameraData>();
        }

        private void Update() {
            if (!isGestionModeActivated) return;
            
            ray = mainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Single.PositiveInfinity, layerMask: GestionModeSelectableLayerMask)) {
                if (targetedGameObject == null) {
                    targetedGameObject = hit.transform.gameObject;
                }

                else {
                    targetedGameObject.layer = 7; // Gestion Mode Selectable layer

                    targetedGameObject = hit.transform.gameObject;
                    targetedGameObject.layer = 11; // Gestion Mode Selected layer
                }

                ObjectsReference.Instance.descriptionsManager.SetDescription(targetedGameObject.GetComponent<Tag>().itemScriptableObject);
                ObjectsReference.Instance.descriptionsManager.ShowPanel(targetedGameObject.GetComponent<Tag>().gameObjectTag);
            }

            else {
                if (targetedGameObject != null) {
                    targetedGameObject.layer = 7; // GestionMode Selectable layer
                    targetedGameObject = null;
                }
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_default_helper();
            }
        }

        public void SwitchToGestionMode() {
            SetOutlineRenderer();

            ObjectsReference.Instance.descriptionsManager.HideAllPanels();
            ObjectsReference.Instance.uiManager.Show_Interface();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0;
            ObjectsReference.Instance.mainCamera.Set0Sensibility();
            
            isGestionModeActivated = true;
        }

        private void SetNormalRenderer() {
            URP_Asset.SetRenderer(0);
        }

        private void SetOutlineRenderer() {
            // render objects in the layer BananaGunSelectable with a white outline
            URP_Asset.SetRenderer(1);
        }

        public void CloseGestionMode() {
            ObjectsReference.Instance.build.CancelGhost();
            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_default_helper();

            SetNormalRenderer();
            
            ObjectsReference.Instance.mainCamera.Switch_To_Camera_View(CAMERA_MODE.PLAYER_VIEW);
            
            ObjectsReference.Instance.uiManager.Hide_Interface();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1;
            ObjectsReference.Instance.mainCamera.SetNormalSensibility();
            
            if (targetedGameObject != null) {
                targetedGameObject.layer = 7; // Gestion Mode Selectable layer
                targetedGameObject.GetComponent<Renderer>().material.color = Color.white;
            }

            isGestionModeActivated = false;
        }
    }
}