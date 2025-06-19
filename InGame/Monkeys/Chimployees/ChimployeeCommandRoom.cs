using InGame.Interactions;
using UI.InGame;
using UnityEngine;

namespace InGame.Monkeys.Chimployees {
    public class ChimployeeCommandRoom : MonoBehaviour {
        [SerializeField] private Transform chimployeeTransform;
        [SerializeField] private Animator animator;
        [SerializeField] private UIFace uiFace;
        [SerializeField] private BubbleDialogue bubbleDialogue;
        [SerializeField] private GameObject capsuleMessage;
        
        private static readonly int isDead = Animator.StringToHash("isDead");
        private static readonly int isTyping = Animator.StringToHash("isTyping");

        [SerializeField] private Transform initialChimployeeTransform;
        
        private void Start() {
            if (ObjectsReference.Instance.bananaGun.bananaGunGameObject.activeInHierarchy) {
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
            
            capsuleMessage.SetActive(false);
        }

        public void SetTutorialChimployeeConfiguration() {
            chimployeeTransform.position = initialChimployeeTransform.position;
            chimployeeTransform.rotation = initialChimployeeTransform.rotation;

            uiFace.Die(true);
            animator.SetBool(isDead, true);
            animator.SetBool(isTyping, false);

            bubbleDialogue.DisableBubble();
            bubbleDialogue.enabled = false;
            
            capsuleMessage.SetActive(true);
        }
    }
}
