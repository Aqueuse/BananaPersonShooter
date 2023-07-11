using UnityEngine;

namespace Monkeys.Gorilla {
    public class GorillaAnimationBehaviour : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var gorilla = animator.GetComponent<GorillaMonkey>();

            if (stateInfo.IsTag("GorillaAttack")) {
                gorilla.gorillaHandLeft.GetComponent<SphereCollider>().enabled = true;
                gorilla.gorillaHandRight.GetComponent<SphereCollider>().enabled = true;
            }
        }


        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var gorilla = animator.GetComponent<GorillaMonkey>();
 
            if (stateInfo.IsTag("GorillaAttack")) {
                gorilla.isAttackingPlayer = false;
                gorilla.gorillaHandLeft.GetComponent<SphereCollider>().enabled = false;
                gorilla.gorillaHandRight.GetComponent<SphereCollider>().enabled = false;
            }

            if (stateInfo.IsName("tourbismash")) {
                gorilla.CreateShockWave();
            }
        }
    }
}