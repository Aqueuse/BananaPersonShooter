using Tags;
using UnityEngine;

namespace Game.Bananas.Regimes {
    enum VerticalState {
        UP = 0,
        DOWN = 1
    }
    
    public class UpDownRegime : MonoBehaviour {
        private VerticalState _verticalState = VerticalState.UP;

        private Vector3 _upPosition;

        private float _step;
        private Vector3 _initialPosition;

        private const float Speed = 5f;

        private void Start() {
            _initialPosition = transform.position;
            _upPosition = _initialPosition + Vector3.up * 100;
        }
        
        private void FixedUpdate() {
            if (!ObjectsReference.Instance.gameManager.isGamePlaying) return;
            
            _step =  Speed * Time.deltaTime; // calculate distance to move

            if (Vector3.Distance(transform.position, _initialPosition) <= 1f) {
                _verticalState = VerticalState.UP;
            }

            if (Vector3.Distance(transform.position, _upPosition) <= 1f) {
                _verticalState = VerticalState.DOWN;
            }
            
            if (_verticalState == VerticalState.UP) {
                transform.position = Vector3.MoveTowards(transform.position, _upPosition, _step);
            }
            
            if (_verticalState == VerticalState.DOWN) {
                transform.position = Vector3.MoveTowards(transform.position, _initialPosition, _step);
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (!TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.BUILD_UNVALID)) {
                _verticalState = _verticalState == VerticalState.UP ? VerticalState.DOWN : VerticalState.UP;
            }
        }
    }
}
