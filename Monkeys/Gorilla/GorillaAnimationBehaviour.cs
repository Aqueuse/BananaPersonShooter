using Game;
using UnityEngine;

namespace Monkeys.Gorilla {
    public class GorillaAnimationBehaviour : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsTag("GorillaAttack")) {
                MapsManager.Instance.currentMap.activeMonkey.GetComponent<GorillaMonkey>().gorillaHandLeft.GetComponent<SphereCollider>().enabled = true;
                MapsManager.Instance.currentMap.activeMonkey.GetComponent<GorillaMonkey>().gorillaHandRight.GetComponent<SphereCollider>().enabled = true;
            }
        }


        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var gorilla = GameObject.FindWithTag("Boss").GetComponent<GorillaMonkey>();
 
            if (stateInfo.IsTag("GorillaAttack")) {
                gorilla.isAttackingPlayer = false;
                MapsManager.Instance.currentMap.activeMonkey.GetComponent<GorillaMonkey>().gorillaHandLeft.GetComponent<SphereCollider>().enabled = false;
                MapsManager.Instance.currentMap.activeMonkey.GetComponent<GorillaMonkey>().gorillaHandRight.GetComponent<SphereCollider>().enabled = false;
            }

            if (stateInfo.IsName("tourbismash")) {
                gorilla.CreateShockWave();
            }
        }
    }
}