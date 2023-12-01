using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UIFace : MonoBehaviour {
        [SerializeField] private Animator _faceAnimator;
        private static readonly int Move = Animator.StringToHash("MOVE");
        private static readonly int Hurted = Animator.StringToHash("HURTED");
        private static readonly int Dead = Animator.StringToHash("DEAD");
        private static readonly int Horrified = Animator.StringToHash("HORRIFIED");
        private static readonly int Happy = Animator.StringToHash("HAPPY");
        private static readonly int Intrigued = Animator.StringToHash("INTRIGUED");
        private static readonly int GooglyEyes = Animator.StringToHash("GOOGLY_EYES");

        [SerializeField] private Image eyeLeft;
        [SerializeField] private Image eyePupilLeft;
        [SerializeField] private Image eyeLidLeft;
        [SerializeField] private Image eyeRight;
        [SerializeField] private Image eyePupilRight;
        [SerializeField] private Image eyeLidRight;

        [SerializeField] private TextMeshProUGUI deadEyeLeft;
        [SerializeField] private TextMeshProUGUI deadEyeRight;
        [SerializeField] private TextMeshProUGUI disorientedMouth;
        
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
            eyeLeft.enabled = !isDead;
            eyePupilLeft.enabled = !isDead;
            eyeRight.enabled = !isDead;
            eyePupilRight.enabled = !isDead;
            eyeLidLeft.enabled = !isDead;
            eyeLidRight.enabled = !isDead;

            deadEyeLeft.enabled = isDead;
            deadEyeRight.enabled = isDead;
            disorientedMouth.enabled = isDead;
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
