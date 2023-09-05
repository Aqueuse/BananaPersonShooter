using Game.CommandRoomPanelControls;
using Items;
using UI.InGame;
using UnityEngine;

namespace Monkeys.Chimployees {
    public class ChimployeeCommandRoom : Chimployee {
        [SerializeField] private RuntimeAnimatorController initialAnimatorCharacterController;
        [SerializeField] private RuntimeAnimatorController apeRessourcesCharacterController;

        [SerializeField] private Transform chimployeeTransform;
        [SerializeField] private Animator animator;
        [SerializeField] private UIFace uiFace;
        [SerializeField] private BubbleDialogue bubbleDialogue;

        private void Start() {
            animator = GetComponent<Animator>();

            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                SetNormalChimployeeConfiguration();
            }
        }

        private void SetNormalChimployeeConfiguration() {
            animator.runtimeAnimatorController = apeRessourcesCharacterController;
            chimployeeTransform.position = CommandRoomControlPanelsManager.Instance.apeResourcesChimployeeTransform.position;
            chimployeeTransform.rotation = CommandRoomControlPanelsManager.Instance.apeResourcesChimployeeTransform.rotation;

            uiFace.Die(false);

            enabled = false;
            bubbleDialogue.enabled = true;
            bubbleDialogue.EnableBubble();
        }

        public void SetInitialChimployeeConfiguration() {
            animator.runtimeAnimatorController = initialAnimatorCharacterController;
            uiFace.Die(true);

            bubbleDialogue.DisableBubble();
            bubbleDialogue.enabled = false;
        }
    }
}
