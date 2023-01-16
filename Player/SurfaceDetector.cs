using Audio;
using Enums;
using UnityEngine;

namespace Player {
    public class SurfaceDetector : MonoSingleton<SurfaceDetector> {
        private int _layerMask;
        private Vector3 bananaManPosition;

        [SerializeField] private GenericDictionary<Material, FootStepType> basicFootStepTypesByMaterial;

        [SerializeField] GenericDictionary<Material, FootStepType> terrainFootStepTypeByMaterial;

        private void Start() {
            _layerMask = 1 << 9;
        }

        void Update() {
            if (GameManager.Instance.isGamePlaying) {
                if (Physics.CheckSphere(transform.position, 0.25f, _layerMask)) {
                    BananaMan.Instance.isInAir = false;
                    BananaMan.Instance.tpsPlayerAnimator.IsInAir(false);
                    BananaMan.Instance.tpsPlayerAnimator.IsGrounded(true);
                    BananaMan.Instance.lastY = BananaMan.Instance.transform.position.y;

                    if (Physics.Raycast(transform.position, -transform.up, out RaycastHit raycastHit, 5, layerMask:_layerMask)) {
                        if (raycastHit.transform.GetComponent<Renderer>().sharedMaterials.Length <= 2) {
                            AudioManager.Instance.footStepType = basicFootStepTypesByMaterial[raycastHit.transform.GetComponent<Renderer>().sharedMaterials[0]];
                        }

                        else {
                            AudioManager.Instance.footStepType = terrainFootStepTypeByMaterial[
                                GetSurfaceTypeFromMaterial(raycastHit.transform.gameObject, raycastHit.triangleIndex)];
                        }
                    }
                }

                else {
                    BananaMan.Instance.isInAir = true;
                    BananaMan.Instance.tpsPlayerAnimator.IsInAir(true);
                    BananaMan.Instance.tpsPlayerAnimator.IsGrounded(false);
                }
            }
        }

        private Material GetSurfaceTypeFromMaterial(GameObject obj, int triangleIndex) {
            if (obj.TryGetComponent(out GameObjectData gameObjectData)) {
                return gameObjectData.dataMapMat.GetMaterial(triangleIndex);
            }
            else {
                Renderer terrainRenderer = obj.GetComponent<Renderer>();

                if (terrainRenderer.materials.Length > 2) {
                    Debug.Log("renderer.materials.Length > 2");
                    return null;
                }

                return terrainRenderer.material;
            }
        }
    }
}