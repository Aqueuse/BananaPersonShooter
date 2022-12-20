using System.Collections;
using UnityEngine;

namespace Cameras {
    public class CameraShake : MonoSingleton<CameraShake> {
        private Vector3 _originalPos;
        private float _timeAtCurrentFrame;
        private float _timeAtLastFrame;
        private float _fakeDelta;

        void Update() {
            // Calculate a fake delta time, so we can Shake while game is paused.
            _timeAtCurrentFrame = Time.realtimeSinceStartup;
            _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
            _timeAtLastFrame = _timeAtCurrentFrame;
        }

        public void Shake(float duration, float amount) {
            _originalPos = gameObject.transform.localPosition;
            StopAllCoroutines();
            StartCoroutine(ShakeCoroutine(duration, amount));
        }

        public IEnumerator ShakeCoroutine(float duration, float amount) {
            while (duration > 0) {
                transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

                duration -= _fakeDelta;

                yield return null;
            }

            transform.localPosition = _originalPos;
        }
    }
}