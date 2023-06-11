using Player;
using UnityEngine;

namespace Building {
    public class BananaGun : MonoBehaviour {
        public GameObject moverTarget;
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
            ObjectsReference.Instance.bananaGun.transform.LookAt(moverTarget.transform, Vector3.up);

            if (ObjectsReference.Instance.gameActions.isBuildModeActivated) {
                if (Physics.Raycast(ObjectsReference.Instance.gameManager.cameraMain.transform.position, ObjectsReference.Instance.gameManager.cameraMain.transform.forward, out RaycastHit raycastHit, 100, layerMask:aspirableLayerMask)) {
                    UnhighlightSelectedObject();
                    targetedGameObject = raycastHit.transform.gameObject;
                    HighlightSelectedObject();
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
            wasFocus = _playerController.isFocusCamera;
        }

        public void UngrabBananaGun() {
            ObjectsReference.Instance.bananaGunPut.CancelThrow();
            
            bananaGun.SetActive(false);
            bananaGunInBack.SetActive(true);

            ObjectsReference.Instance.bananaMan.isGrabingBananaGun = false;
            ObjectsReference.Instance.bananaMan.tpsPlayerAnimator.FocusCamera(wasFocus);
        }
        
        private void HighlightSelectedObject() {
            if (targetedGameObject != null && targetedGameObject.layer == 7) {
                aspirableMaterials = targetedGameObject.GetComponent<Renderer>().materials;
                aspirableMaterials[0].SetFloat(Emission, 0.2f);
                targetedGameObject.GetComponent<Renderer>().materials = aspirableMaterials;
            }
        }

        public void UnhighlightSelectedObject() {
            if (targetedGameObject != null && targetedGameObject.layer == 7) {
                aspirableMaterials = targetedGameObject.GetComponent<Renderer>().materials;
                aspirableMaterials[0].SetFloat(Emission, 0f);
                targetedGameObject.GetComponent<Renderer>().materials = aspirableMaterials;
            }
        }
    }
}