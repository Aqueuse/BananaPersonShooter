using InGame.Interactions;
using UI.InGame;
using UnityEngine;

namespace InGame.Monkeys.Chimployees {
    public class ChimployeeCommandRoom : Chimployee {
        [SerializeField] private Transform chimployeeTransform;
        [SerializeField] private Animator animator;
        [SerializeField] private UIFace uiFace;
        [SerializeField] private BubbleDialogue bubbleDialogue;

        private readonly string beDeadAnimatorProperty = "isDead";
        private readonly string isTypingAnimatorProperty = "isTyping";

        private void Start() {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                SetNormalChimployeeConfiguration();
            }
            else {
                SetTutorialChimployeeConfiguration();
            }
        }

        private void SetNormalChimployeeConfiguration() {
            chimployeeTransform.position = ObjectsReference.Instance.commandRoomControlPanelsManager.chairLifeSimulatorTransform.position;
            chimployeeTransform.rotation = ObjectsReference.Instance.commandRoomControlPanelsManager.chairLifeSimulatorTransform.rotation;

            uiFace.Die(false);
            animator.SetBool(beDeadAnimatorProperty, false);
            animator.SetBool(isTypingAnimatorProperty, true);

            enabled = false;
            bubbleDialogue.enabled = true;
            bubbleDialogue.EnableBubble();
        }

        public void SetTutorialChimployeeConfiguration() {
            uiFace.Die(true);
            animator.SetBool(beDeadAnimatorProperty, true);
            animator.SetBool(isTypingAnimatorProperty, false);

            bubbleDialogue.DisableBubble();
            bubbleDialogue.enabled = false;
        }
    }
}
