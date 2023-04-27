using Data;
using Enums;
using UnityEngine;

namespace Player {
	public class SurfaceDetector : MonoBehaviour {
		[SerializeField] private FootstepSoundsByMaterialScriptableObject footstepSoundsByMaterialScriptableObject;
		[SerializeField] private LayerMask surfaceLayerMask;
		
		private RaycastHit raycastHit;
		
		private void Update() {
			if (ObjectsReference.Instance.gameManager.isGamePlaying) {
				if (Physics.Raycast(transform.position, -transform.up, out raycastHit, 5, layerMask: surfaceLayerMask)) {
					var check = raycastHit.transform.GetComponent<Renderer>();
					if (check.sharedMaterials.Length <= 2) {
						if (!footstepSoundsByMaterialScriptableObject.basicFootStepTypesByMaterial.ContainsKey(check.sharedMaterials[0])) {
							ObjectsReference.Instance.audioManager.footStepType = FootStepType.ROCK;
						}

						else {
							ObjectsReference.Instance.audioManager.footStepType = footstepSoundsByMaterialScriptableObject.basicFootStepTypesByMaterial[check.sharedMaterials[0]];
						}
					}

					else {
						ObjectsReference.Instance.audioManager.footStepType = footstepSoundsByMaterialScriptableObject.terrainFootStepTypeByMaterial[
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