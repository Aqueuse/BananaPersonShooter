using Audio;
using Enums;
using Game;
using UnityEngine;

namespace Player {
    public class SurfaceDetector : MonoSingleton<SurfaceDetector> {
        [SerializeField] private GenericDictionary<Material, FootStepType> basicFootStepTypesByMaterial;
        [SerializeField] GenericDictionary<Material, FootStepType> terrainFootStepTypeByMaterial;

        private int _layerMask;

        private void Start() {
            _layerMask = 1 << 9;
        }

        private void Update() {
                if (!BananaMan.Instance.isInAir && GameManager.Instance.isGamePlaying) {
                    if (Physics.Raycast(transform.position, -transform.up, out RaycastHit raycastHit, 5, layerMask:_layerMask)) {
                        if (raycastHit.transform.GetComponent<Renderer>().sharedMaterials.Length <= 2) {
                            if (!basicFootStepTypesByMaterial.ContainsKey(raycastHit.transform.GetComponent<Renderer>()
                                    .sharedMaterials[0])) {
                                AudioManager.Instance.footStepType = FootStepType.ROCK;
                            }

                            else {
                                AudioManager.Instance.footStepType = basicFootStepTypesByMaterial[raycastHit.transform.GetComponent<Renderer>().sharedMaterials[0]];
                            }
                        }

                        else {
                            AudioManager.Instance.footStepType = terrainFootStepTypeByMaterial[
                                GetSurfaceTypeFromMaterial(raycastHit.transform.gameObject, raycastHit.triangleIndex)];
                        }
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