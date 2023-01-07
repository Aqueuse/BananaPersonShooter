using UnityEngine;

namespace Player {
    public class DetectWater : MonoBehaviour {
        private const int LayerMask = 1 << 4;
        private PlayerController _playerController;

        private void Start() {
            _playerController = BananaMan.Instance.GetComponent<PlayerController>();
        }

        void Update() {
            if (GameManager.Instance.isGamePlaying) {
                if (Physics.CheckSphere(transform.position, 0.25f, LayerMask)) {
                    BananaMan.Instance.isInWater = true;
                    _playerController.baseMovementSpeed = 4f;
                }

                else {
                    if (_playerController.baseMovementSpeed < 6f) {
                        BananaMan.Instance.isInWater = false;
                        _playerController.baseMovementSpeed = 6;
                    }
                }
            }
        }
    }
}
