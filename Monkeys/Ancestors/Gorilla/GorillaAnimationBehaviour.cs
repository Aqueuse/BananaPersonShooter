using UnityEngine;

namespace Monkeys.Ancestors.Gorilla {
    public class GorillaAnimationBehaviour : StateMachineBehaviour {

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var gorilla = animator.GetComponent<GorillaMonkey>();
            var generalMonkeyClass = animator.GetComponent<Monkey>(); 
 
            if (stateInfo.IsTag("GorillaAttack")) {
                gorilla.isAttackingPlayer = false;
            }

            if (stateInfo.IsName("tourbismash")) {
                gorilla.CreateShockWave();
            }

            if (stateInfo.IsName("grab")) {
                generalMonkeyClass.Eat();
            }
        }
    }
}