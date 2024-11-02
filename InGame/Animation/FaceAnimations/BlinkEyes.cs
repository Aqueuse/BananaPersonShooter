using System.Collections;
using UnityEngine;

namespace InGame.Animation.FaceAnimations {
    public class BlinkEyes : MonoBehaviour {
        [SerializeField] private Transform eyeLeftTransform;
        [SerializeField] private Transform eyeRightTransform;
        [SerializeField] private float blinkFrequency = 1f;
        [SerializeField] private float blinkTime = 0.1f;
        
        private void Start() {
            eyeLeftTransform.localScale = Vector3.one;
            eyeRightTransform.localScale = Vector3.one;
            
            StartCoroutine(ResizeCoroutine());
        }
        
        private IEnumerator ResizeCoroutine() {
            while (true) {
                yield return ResizeToOne();

                yield return new WaitForSeconds(blinkFrequency);

                yield return ResizeToZero();
                
                yield return new WaitForSeconds(blinkTime);
            }
        }
        
        private IEnumerator ResizeToZero() {
            eyeLeftTransform.localScale = Vector3.zero;
            eyeRightTransform.localScale = Vector3.zero;
            yield return null;
        }

        private IEnumerator ResizeToOne() {
            eyeLeftTransform.localScale = Vector3.one;
            eyeRightTransform.localScale = Vector3.one;

            yield return null;
        }
    }
}
