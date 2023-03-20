using UnityEngine;

namespace Player
{
	public class PlayerAnimationsBehavior : StateMachineBehaviour
	{

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (stateInfo.IsTag("throw"))
				BananaMan.Instance.tpsPlayerAnimator.ExitArmsOnlyLayer();

			if (stateInfo.IsTag("searching"))
				BananaMan.Instance.tpsPlayerAnimator.ExitArmsOnlyLayer();


			if (stateInfo.IsTag("roll")/* || stateInfo.IsName("standing jump end") || stateInfo.IsName("jump when sprint end")*/)
			{
				BananaMan.Instance.GetComponent<PlayerController>().isRolling = false;
				BananaMan.Instance.GetComponent<CharacterController>().height = 1.82f;
				BananaMan.Instance.GetComponent<CharacterController>().center = new Vector3(0, 0.88f, 0);
				BananaMan.Instance.GetComponent<CapsuleCollider>().height = 1.85f;
			}

			if (stateInfo.IsName("standing jump end") || stateInfo.IsName("jump when sprint end"))
				BananaMan.Instance.tpsPlayerAnimator.IsGrounded(true);

		}

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (stateInfo.IsName("jump when sprint start") || stateInfo.IsName("standing jump start"))
				BananaMan.Instance.tpsPlayerAnimator.IsGrounded(false);

			if (stateInfo.IsTag("roll"))
				BananaMan.Instance.GetComponent<PlayerController>().isRolling = true;
		}
	}
}
