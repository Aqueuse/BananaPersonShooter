using UnityEngine;

namespace Monkeys.Ancestors.Gorilla {
    public class GorillaAnimationBehaviour : StateMachineBehaviour {

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var gorilla = animator.GetComponent<GorillaMonkey>();
 
            if (stateInfo.IsTag("GorillaAttack")) {
                gorilla.isAttackingPlayer = false;
            }

            if (stateInfo.IsName("tourbismash")) {
                gorilla.CreateShockWave();
            }
        }
    }
}