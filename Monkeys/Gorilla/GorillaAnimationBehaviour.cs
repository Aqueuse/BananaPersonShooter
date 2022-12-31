using UnityEngine;

namespace Monkeys.Gorilla {
    public class GorillaAnimationBehaviour : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsTag("GorillaAttack")) {
                GameObject.FindWithTag("GorillaHandLeft").GetComponent<SphereCollider>().enabled = true;
                GameObject.FindWithTag("GorillaHandRight").GetComponent<SphereCollider>().enabled = true;
            }
        }


        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var gorilla = GameObject.FindWithTag("Boss").GetComponent<GorillaMonkey>();
 
            if (stateInfo.IsTag("GorillaAttack")) {
                gorilla.isAttackingPlayer = false;
                GameObject.FindWithTag("GorillaHandLeft").GetComponent<SphereCollider>().enabled = false;
                GameObject.FindWithTag("GorillaHandRight").GetComponent<SphereCollider>().enabled = false;
            }

            if (stateInfo.IsName("tourbismash")) {
                gorilla.CreateShockWave();
            }
        }
    }
}