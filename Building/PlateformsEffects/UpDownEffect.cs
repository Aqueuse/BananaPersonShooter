using Game;
using UnityEngine;

namespace Building.PlateformsEffects {
    enum PlateformVerticalState {
        UP,
        DOWN
    }
    
    public class UpDownEffect : MonoBehaviour {
        private Vector3 upPosition;
        
        private PlateformVerticalState _plateformVerticalState = PlateformVerticalState.UP;

        private float step;
        private Vector3 initialPosition;
        private Vector3 bananaManPosition;
        
        private bool isActive;
        private readonly float speed = 10f;
        private int _upDownCount = 10;

        private void Start() {
            initialPosition = transform.position;
            upPosition = initialPosition + Vector3.up * 100;

            isActive = false;
            _upDownCount = 10;
        }

        private void FixedUpdate() {
            if (GameManager.Instance.isGamePlaying) {
                if (isActive && _upDownCount > 0) {
                    step =  speed * Time.deltaTime; // calculate distance to move

                    if (Vector3.Distance(transform.position, initialPosition) <= 1f) {
                        _plateformVerticalState = PlateformVerticalState.UP;
                        _upDownCount--;
                    }

                    if (Vector3.Distance(transform.position, upPosition) <= 1f) {
                        _plateformVerticalState = PlateformVerticalState.DOWN;
                    }
                
                    if (_plateformVerticalState == PlateformVerticalState.UP) {
                        transform.position = Vector3.MoveTowards(transform.position, upPosition, step);
                    }
                
                    if (_plateformVerticalState == PlateformVerticalState.DOWN) {
                        transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);
                    }
                }

                if (isActive && _upDownCount <= 0) {
                    isActive = false;
                    GetComponent<Plateform>().ResetMaterial();
                }
            }
        }

        public void Activate() {
            if (!isActive) {
                _upDownCount = 10;
                isActive = true;
            }
        }
    }
}
