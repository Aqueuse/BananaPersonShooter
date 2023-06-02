using UnityEngine;

namespace Player {
	public class PlayerAnimationsBehaviour : StateMachineBehaviour {
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			if (stateInfo.IsTag("roll")) {
				ObjectsReference.Instance.bananaMan.GetComponent<PlayerController>().isRolling = false;
				ObjectsReference.Instance.bananaMan.GetComponent<CapsuleCollider>().height = 1.807042f;
				ObjectsReference.Instance.bananaMan.GetComponent<CapsuleCollider>().center = new Vector3(-0.01361084f, 0.9f, 1.027142e-11f);
			}
		}

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			if (stateInfo.IsTag("roll"))
				ObjectsReference.Instance.bananaMan.GetComponent<PlayerController>().isRolling = true;
		}
	}
}
