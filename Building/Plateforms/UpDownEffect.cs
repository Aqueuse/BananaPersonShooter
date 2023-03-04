using Game;
using UnityEngine;

namespace Building.Plateforms {
    enum VerticalState {
        UP,
        DOWN
    }
    
    public class UpDownEffect : MonoBehaviour {
        private VerticalState verticalState = VerticalState.UP;

        private Vector3 upPosition;

        private float step;
        private Vector3 initialPosition;
        
        private readonly float speed = 5f;
        
        private void Start() {
            initialPosition = transform.position;
            upPosition = initialPosition + Vector3.up * 100;
        }
        
        private void FixedUpdate() {
            if (GameManager.Instance.isGamePlaying) {
                step =  speed * Time.deltaTime; // calculate distance to move

                if (Vector3.Distance(transform.position, initialPosition) <= 1f) {
                    verticalState = VerticalState.UP;
                }

                if (Vector3.Distance(transform.position, upPosition) <= 1f) {
                    verticalState = VerticalState.DOWN;
                }
            
                if (verticalState == VerticalState.UP) {
                    transform.position = Vector3.MoveTowards(transform.position, upPosition, step);
                }
            
                if (verticalState == VerticalState.DOWN) {
                    transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);
                }
            }
        }
    }
}