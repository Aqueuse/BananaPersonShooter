using Unity.Mathematics;
using UnityEngine;

namespace InGame.Player {
	public class FootStepSurfaceDetector : MonoBehaviour {
		[SerializeField] private LayerMask surfacesLayerMask;
		public FootStepType footStepType;

		private RaycastHit _raycastHit;
		
		[SerializeField] private Transform raycastOrigin;
    
		private Vector2[] uv;
		private Vector3[] vertices;

		private int closestVertexIndex;
		public int footstepGridSize = 3;
		
		private void FixedUpdate() {
			if (ObjectsReference.Instance.gameManager.gameContext != GameContext.IN_GAME) return;

			if (Physics.Raycast(origin: raycastOrigin.position, direction: Vector3.down, out RaycastHit raycastHit, maxDistance: 2, layerMask: surfacesLayerMask)) {
				footStepType = CheckFootstepType(raycastHit.textureCoord);
			}
		}

		private FootStepType CheckFootstepType(Vector3 hitUvPosition) {
			var Xcoordinates = math.floor(hitUvPosition.x * footstepGridSize); 
			var Ycoordinates = math.floor((1-hitUvPosition.y) * footstepGridSize); // the origin is left bottom, so we need to invert Y

			// x + ( y * gridCount ) = index
			var soundIndex =  Xcoordinates + (Ycoordinates * footstepGridSize);
			
			if (soundIndex >= 0 && soundIndex <= ObjectsReference.Instance.audioManager.audioFootStepsDictionnary.Count) {
				return (FootStepType)soundIndex;
			}
			
			return FootStepType.ROCK;
		}
	}
}