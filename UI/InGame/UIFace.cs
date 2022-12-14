using UnityEngine;


namespace UI.InGame {
    public class UIFace : MonoSingleton<UIFace> {
        private Canvas _faceCanvas;
        private CanvasGroup _faceCanvasGroup;
        private Camera _mainCamera;
        private Animator _faceAnimator;
        private static readonly int Move = Animator.StringToHash("MOVE");
        private static readonly int Hurted = Animator.StringToHash("HURTED");
        private static readonly int Dead = Animator.StringToHash("DEAD");
        private static readonly int Horrified = Animator.StringToHash("HORRIFIED");
        private static readonly int Happy = Animator.StringToHash("HAPPY");
        private static readonly int Intrigued = Animator.StringToHash("INTRIGUED");
        private static readonly int GooglyEyes = Animator.StringToHash("GOOGLY_EYES");

        private void Start() {
            _faceCanvas = GetComponent<Canvas>();
            _faceCanvasGroup = GetComponent<CanvasGroup>();
            _mainCamera = Camera.main;
            _faceAnimator = GetComponent<Animator>();
        }

        void Update() {
            if (Vector3.Dot(_mainCamera.transform.forward, _faceCanvas.transform.forward) > 0) {
                _faceCanvasGroup.alpha = 0;
            }

            else _faceCanvasGroup.alpha = 1;
        }

        public void MoveFaceAnimation(float speed) {
            _faceAnimator.SetFloat(Move, speed);
        }

        public void GetHurted(bool isHurted) {
            _faceAnimator.SetBool(Hurted, isHurted);
        }

        public void GetGooglyEyes() {
            _faceAnimator.SetTrigger(GooglyEyes);
        }

        public void Die(bool isDead) {
            _faceAnimator.SetBool(Dead, isDead);
        }

        public void GetHorrified() {
            _faceAnimator.SetTrigger(Horrified);
        }

        public void GetHappy() {
            _faceAnimator.SetBool(Happy, true);
            Invoke(nameof(GetNeutral), 60);
        }

        public void GetNeutral() {
            _faceAnimator.SetBool(Happy, false);
        }

        public void GetIntrigued() {
            _faceAnimator.SetTrigger(Intrigued);
        }
    }
}
