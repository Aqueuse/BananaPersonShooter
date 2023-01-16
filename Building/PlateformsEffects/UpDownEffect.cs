using UnityEngine;

namespace Building.PlateformsEffects {
    enum PlateformVerticalState {
        UP,
        DOWN
    }
    
    public class UpDownEffect : MonoBehaviour {
        private Vector3 upPosition;

        private bool isActive;
        
        private PlateformVerticalState _plateformVerticalState = PlateformVerticalState.UP;

        private float step;
        private Vector3 initialPosition;
        private Vector3 bananaManPosition;
        
        private readonly float speed = 10f;

        private void Start() {
            initialPosition = transform.position;
            upPosition = initialPosition + Vector3.up * 100;
        }

        private void FixedUpdate() {
            if (isActive) {
                step =  speed * Time.deltaTime; // calculate distance to move

                if (Vector3.Distance(transform.position, initialPosition) <= 1f) {
                    _plateformVerticalState = PlateformVerticalState.UP;
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
        }

        public void Activate() {
            Invoke(nameof(Desactivate), 30f);    
        }

        public void Desactivate() {
            isActive = false;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Banana")) {
                isActive = true;
            }
        }
    }
}
