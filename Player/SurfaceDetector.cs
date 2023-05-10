using Enums;
using TerrainDetection;
using UnityEngine;

namespace Player {
	public class SurfaceDetector : MonoBehaviour {
		private RaycastHit _raycastHit;

		private void Update() {
			if (!ObjectsReference.Instance.gameManager.isGamePlaying) return;
			if (!Physics.Raycast(transform.position, -transform.up, out _raycastHit, 5)) return;

			if (
				_raycastHit.transform.gameObject.GetComponent<MeshCollider>() != null &&
			    _raycastHit.transform.gameObject.GetComponent<TerrainType>() != null) {
				ObjectsReference.Instance.audioManager.footStepType = _raycastHit.transform.gameObject.GetComponent<TerrainType>().footStepType;
			}

			else {
					ObjectsReference.Instance.audioManager.footStepType = FootStepType.ROCK;
			}
		}
	}
}