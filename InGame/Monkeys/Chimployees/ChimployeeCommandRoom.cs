using InGame.Interactions;
using UI.InGame;
using UnityEngine;

namespace InGame.Monkeys.Chimployees {
    public class ChimployeeCommandRoom : MonoBehaviour {
        [SerializeField] private Transform chimployeeTransform;
        [SerializeField] private Animator animator;
        [SerializeField] private UIFace uiFace;
        [SerializeField] private BubbleDialogue bubbleDialogue;
        private static readonly int isDead = Animator.StringToHash("isDead");
        private static readonly int isTyping = Animator.StringToHash("isTyping");
        
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
            animator.SetBool(isDead, false);
            animator.SetBool(isTyping, true);

            enabled = false;
            bubbleDialogue.enabled = true;
            bubbleDialogue.EnableBubble();
        }

        public void SetTutorialChimployeeConfiguration() {
            uiFace.Die(true);
            animator.SetBool(isDead, true);
            animator.SetBool(isTyping, false);

            bubbleDialogue.DisableBubble();
            bubbleDialogue.enabled = false;
        }
    }
}
