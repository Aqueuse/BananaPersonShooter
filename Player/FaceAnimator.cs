using UnityEngine;

namespace Player {
    public class FaceAnimator : MonoSingleton<FaceAnimator> {
        private Animator _faceAnimator;
        private static readonly int Horrified = Animator.StringToHash("HORRIFIED");

        private void Start() {
            _faceAnimator = GetComponent<Animator>();
        }

        public void GetHorrified() {
            _faceAnimator.SetTrigger(Horrified);
        }

    }
}

