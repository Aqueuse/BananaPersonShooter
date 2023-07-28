using Player;
using UnityEngine;

namespace Building {
    public class BananaGun : MonoBehaviour {
        [SerializeField] private Transform bananaGunTargetTransform;
        [SerializeField] private LayerMask aspirableLayerMask;
        private PlayerController _playerController;
        
        public GameObject bananaGun;
        public GameObject bananaGunInBack;
        public GameObject targetedGameObject;
        public bool wasFocus;
        
        private Material[] aspirableMaterials;
        private static readonly int Emission = Shader.PropertyToID("_emission");

        private void Start() {
            _playerController = ObjectsReference.Instance.playerController;
        }
        
        private void Update() {
            if (!ObjectsReference.Instance.bananaMan.isGrabingBananaGun) return;
            bananaGun.gameObject.transform.LookAt(bananaGunTargetTransform, Vector3.up);

            if (ObjectsReference.Instance.gameActions.isBuildModeActivated) {
                if (Physics.Raycast(ObjectsReference.Instance.gameManager.cameraMain.transform.position, ObjectsReference.Instance.gameManager.cameraMain.transform.forward, out var raycastHit, 100, layerMask:aspirableLayerMask)) {
                    UnhighlightSelectedObject();
                    targetedGameObject = raycastHit.transform.gameObject;
                    if (targetedGameObject.layer == 7) {
                        HighlightSelectedObject();
                        ObjectsReference.Instance.uihud.GetCurrentUIHelper().Show_retrieve_confirmation();
                    }
                }
                else {
                    UnhighlightSelectedObject();
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
        }

        public void UngrabBananaGun() {
            ObjectsReference.Instance.bananaGunPut.CancelThrow();
            
            bananaGun.SetActive(false);
            bananaGunInBack.SetActive(true);

            ObjectsReference.Instance.bananaMan.isGrabingBananaGun = false;
            ObjectsReference.Instance.gameActions.isBuildModeActivated = false;
            ObjectsReference.Instance.bananaMan.tpsPlayerAnimator.FocusCamera(wasFocus);
            ObjectsReference.Instance.bananaMan.GetComponent<PlayerIK>().SetAimConstraint(false);
            
            ObjectsReference.Instance.slotSwitch.CancelGhost();
            ObjectsReference.Instance.uihud.GetCurrentUIHelper().show_default_helper();
        }
        
        private void HighlightSelectedObject() {
            aspirableMaterials = targetedGameObject.GetComponent<Renderer>().materials;
            aspirableMaterials[0].SetFloat(Emission, 0.2f);
            targetedGameObject.GetComponent<Renderer>().materials = aspirableMaterials;
            ObjectsReference.Instance.uihud.GetCurrentUIHelper().Show_retrieve_confirmation();
        }

        public void UnhighlightSelectedObject() {
            if (targetedGameObject != null && targetedGameObject.layer == 7) {
                aspirableMaterials = targetedGameObject.GetComponent<Renderer>().materials;
                aspirableMaterials[0].SetFloat(Emission, 0f);
                targetedGameObject.GetComponent<Renderer>().materials = aspirableMaterials;
                ObjectsReference.Instance.uihud.GetCurrentUIHelper().Hide_retrieve_confirmation();
            }
        }
    }
}