using UnityEngine;

namespace Player {
    public class SurfaceDetector : MonoBehaviour {
        private int _layerMask;

        private void Start() {
            _layerMask = 1 << 9;
        }

        void Update() {
            if (Physics.CheckSphere(transform.position, 0.25f, _layerMask)) {
                BananaMan.Instance.isInAir = false;
                BananaMan.Instance.tpsPlayerAnimator.IsInAir(false);
            }

            else {
                BananaMan.Instance.tpsPlayerAnimator.IsInAir(true);
                if (!Physics.Raycast(transform.position, Vector3.down, 2000, _layerMask)) {
                    var transformPosition = transform.position;
                    transformPosition.y = Terrain.activeTerrain.SampleHeight(BananaMan.Instance.transform.position);
                    BananaMan.Instance.transform.position = transformPosition;
                }
            }
        }
    }
}