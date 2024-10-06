using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using Tags;
using UnityEngine;

namespace InGame.Player {
    public class DetectColliders : MonoBehaviour {
        [SerializeField] private LayerMask gestionSelectableLayerMask;

        private PlayerController _playerController;
        private int _waterLayer;
        
        private void Start() {
            _playerController = ObjectsReference.Instance.playerController;

            _waterLayer = LayerMask.NameToLayer("Water");
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer == _waterLayer) {
                _playerController.isInWater = true;
                _playerController.speed = 6f;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.layer == _waterLayer) {
                _playerController.isInWater = false;
            }
        }
    }
}