using System;
using Enums;
using Player;
using Tags;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Building {
    public class BananaGun : MonoBehaviour {
        [SerializeField] private Transform bananaGunTargetTransform;
        [SerializeField] private LayerMask bananaGunSelectableLayerMask;

        [SerializeField] private Material vertexColorGraphVariantMaterial;
        [SerializeField] private Material selectedMaterial;
        
        public GameObject bananaGun;
        public GameObject bananaGunInBack;
        public GameObject targetedGameObject;
        public bool wasFocus;

        public bool isMouseOverUI;
        
        private Vector3 bananaManRotation;
        private Vector3 targetPosition;
        
        private PlayerController _playerController;
        private UniversalAdditionalCameraData URP_Asset;
        
        private void Start() {
            _playerController = ObjectsReference.Instance.playerController;
            URP_Asset = GetComponent<UniversalAdditionalCameraData>();
        }

        private void Update() {
            if (!ObjectsReference.Instance.bananaMan.isGrabingBananaGun) return;
            bananaGun.gameObject.transform.LookAt(bananaGunTargetTransform, Vector3.up);

            targetPosition = bananaGunTargetTransform.position;

            bananaManRotation.x = targetPosition.x;
            bananaManRotation.y = ObjectsReference.Instance.bananaMan.transform.position.y;
            bananaManRotation.z = targetPosition.z;

            ObjectsReference.Instance.bananaMan.transform.LookAt(bananaManRotation, Vector3.up);

            Ray ray = Camera.main.ScreenPointToRay (UnityEngine.Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, Single.PositiveInfinity, layerMask:bananaGunSelectableLayerMask)) {
                if (targetedGameObject == null) {
                    targetedGameObject = hit.transform.gameObject;
                }

                else {
                    targetedGameObject.layer = 7; // BananaGun Selectable layer
                    targetedGameObject.GetComponent<Renderer>().material = vertexColorGraphVariantMaterial;                    

                    targetedGameObject = hit.transform.gameObject;
                    targetedGameObject.layer = 11; // BananaGun Selected layer
                    targetedGameObject.GetComponent<Renderer>().material = selectedMaterial;
                }

                Debug.Log(targetedGameObject.name);
                
                var gameObjectTag = TagsManager.Instance.GetTag(targetedGameObject);
                ObjectsReference.Instance.descriptionsManager.SetDescriptionByTag(gameObjectTag, targetedGameObject);
                ObjectsReference.Instance.descriptionsManager.ShowPanel(gameObjectTag);
                
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().Show_retrieve_confirmation();
            }
            
            else {
                if (targetedGameObject != null) {
                    targetedGameObject.layer = 7; // BananaGun Selectable layer
                    targetedGameObject.GetComponent<Renderer>().material = vertexColorGraphVariantMaterial;                    
                    targetedGameObject = null;
                }
            }
        }
        
        public void GrabBananaGun() {
            bananaGun.SetActive(true);
            bananaGunInBack.SetActive(false);

            ObjectsReference.Instance.bananaMan.isGrabingBananaGun = true;
            ObjectsReference.Instance.bananaMan.tpsPlayerAnimator.GrabBananaGun();
            ObjectsReference.Instance.bananaMan.GetComponent<PlayerIK>().SetAimConstraint(true);
            wasFocus = _playerController.isFocusCamera;

            SetOutlineRenderer();

            ObjectsReference.Instance.descriptionsManager.HideAllPanels();
            ObjectsReference.Instance.uiManager.Show_Interface();
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0;
            ObjectsReference.Instance.mainCamera.Set0Sensibility();
        }

        public void UngrabBananaGun() {
            ObjectsReference.Instance.bananaGunPut.CancelThrow();
            
            bananaGun.SetActive(false);
            bananaGunInBack.SetActive(true);

            ObjectsReference.Instance.bananaMan.isGrabingBananaGun = false;
            ObjectsReference.Instance.bananaMan.tpsPlayerAnimator.FocusCamera(wasFocus);
            ObjectsReference.Instance.bananaMan.GetComponent<PlayerIK>().SetAimConstraint(false);
            
            ObjectsReference.Instance.slotSwitch.CancelGhost();
            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_default_helper();

            SetNormalRenderer();
            
            ObjectsReference.Instance.uiManager.Hide_Interface();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1;
            ObjectsReference.Instance.mainCamera.SetNormalSensibility();
            
            if (targetedGameObject != null) {
                targetedGameObject.layer = 7; // BananaGun Selectable layer
                targetedGameObject.GetComponent<Renderer>().material = vertexColorGraphVariantMaterial;       
            }
        }
        
        private void SetNormalRenderer() {
            URP_Asset.SetRenderer(0);
        }

        private void SetOutlineRenderer() { // render objects in the layer BananaGunSelectable with a white outline
            URP_Asset.SetRenderer(1);
        }

        public void SetMouseOverUI(bool isOverUI) {
            isMouseOverUI = isOverUI;
        }
    }
}