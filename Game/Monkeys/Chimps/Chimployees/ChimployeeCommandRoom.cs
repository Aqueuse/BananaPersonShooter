using Game.CommandRoomPanelControls;
using Interactions;
using Monkeys.Chimployees;
using UI.InGame;
using UnityEngine;

namespace Game.Monkeys.Chimps.Chimployees {
    public class ChimployeeCommandRoom : Chimployee {
        [SerializeField] private Transform chimployeeTransform;
        [SerializeField] private Animator animator;
        [SerializeField] private UIFace uiFace;
        [SerializeField] private BubbleDialogue bubbleDialogue;

        private readonly int baseLayer = 0;
        private readonly int deadLayer = 1;

        private void Start() {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                SetNormalChimployeeConfiguration();
            }
        }

        private void SetNormalChimployeeConfiguration() {
            chimployeeTransform.position = CommandRoomControlPanelsManager.Instance.apeResourcesChimployeeTransform.position;
            chimployeeTransform.rotation = CommandRoomControlPanelsManager.Instance.apeResourcesChimployeeTransform.rotation;

            uiFace.Die(false);

            animator.SetLayerWeight(baseLayer, 1);
            animator.SetLayerWeight(deadLayer, 0);

            enabled = false;
            bubbleDialogue.enabled = true;
            bubbleDialogue.EnableBubble();
        }

        public void SetTutorialChimployeeConfiguration() {
            uiFace.Die(true);

            animator.SetLayerWeight(baseLayer, 0);
            animator.SetLayerWeight(deadLayer, 1);

            bubbleDialogue.DisableBubble();
            bubbleDialogue.enabled = false;
        }
    }
}
