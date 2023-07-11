using TerrainDetection;
using UnityEngine;

namespace Player {
	public class SurfaceDetector : MonoBehaviour {
		[SerializeField] private LayerMask surfacesLayerMask;
		public FootStepType footStepType;

		private RaycastHit _raycastHit;

		private void Update() {
			if (!ObjectsReference.Instance.gameManager.isGamePlaying) return;
			if (!Physics.Raycast(transform.position, -transform.up, out _raycastHit, 5, layerMask:surfacesLayerMask)) return;
			
			if (_raycastHit.transform.gameObject.GetComponent<TerrainType>() != null) {
				footStepType = _raycastHit.transform.gameObject.GetComponent<TerrainType>().footStepType;
				return;
			}
			
			footStepType = FootStepType.ROCK;
		}
	}
}