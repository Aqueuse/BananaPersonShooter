using UnityEngine;

namespace Game.Monkeys.Ancestors.Gorilla {
    public class GorillaAnimationBehaviour : StateMachineBehaviour {
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var commonMonkeyClass = animator.GetComponent<Monkey>(); 
            
            if (stateInfo.IsName("grab")) {
                commonMonkeyClass.Eat();
            }
        }
    }
}