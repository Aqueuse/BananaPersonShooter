using Player;
using UnityEngine;

namespace Building {
    public class BananaGun : MonoBehaviour {
        public GameObject moverTarget;
        [SerializeField] private LayerMask aspirableLayerMask;
        
        [SerializeField] private Color selectionColor;
        [SerializeField] private Color baseColor = Color.white;
        
        private Material[] aspirableMaterials;

        public GameObject bananaGun;
        public GameObject bananaGunInBack;
        
        private PlayerController _playerController;
        public GameObject _targetedGameObject;

        public bool wasFocus;
        
        private void Start() {
            _playerController = ObjectsReference.Instance.bananaMan.GetComponent<PlayerController>();
        }
        
        private void Update() {
            if (!ObjectsReference.Instance.bananaMan.isGrabingBananaGun) return;
            ObjectsReference.Instance.bananaGun.transform.LookAt(moverTarget.transform, Vector3.up);

            if (ObjectsReference.Instance.gameActions.isBuildModeActivated) {
                if (Physics.Raycast(ObjectsReference.Instance.gameManager.cameraMain.transform.position, ObjectsReference.Instance.gameManager.cameraMain.transform.forward, out RaycastHit raycastHit, 100, layerMask:aspirableLayerMask)) {
                    UnhighlightSelectedObject();
                    _targetedGameObject = raycastHit.transform.gameObject;
                    HighlightSelectedObject();
                }
                else {
                    UnhighlightSelectedObject();
                    _targetedGameObject = null;
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

        public void CancelMover() {
            ObjectsReference.Instance.bananaGunPut.CancelThrow();
            
            bananaGun.SetActive(false);
            bananaGunInBack.SetActive(true);

            ObjectsReference.Instance.bananaMan.isGrabingBananaGun = false;

            ObjectsReference.Instance.bananaMan.tpsPlayerAnimator.FocusCamera(wasFocus);
        }
        
        private void HighlightSelectedObject() {
            if (_targetedGameObject != null && _targetedGameObject.layer == 7) {
                aspirableMaterials = _targetedGameObject.GetComponent<MeshRenderer>().materials;
                aspirableMaterials[0].color = selectionColor;
                _targetedGameObject.GetComponent<MeshRenderer>().materials = aspirableMaterials;
            }
        }

        public void UnhighlightSelectedObject() {
            if (_targetedGameObject != null && _targetedGameObject.layer == 7) {
                aspirableMaterials = _targetedGameObject.GetComponent<MeshRenderer>().materials;
                aspirableMaterials[0].color = baseColor;
                _targetedGameObject.GetComponent<MeshRenderer>().materials = aspirableMaterials;
            }
        }
    }
}