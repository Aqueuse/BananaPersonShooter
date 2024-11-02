using UI.InGame.Merchimps;
using UnityEngine;

namespace InGame.Monkeys.Merchimps {
    public class MerchimpBehaviour : MonoBehaviour {
        public MonkeyMenBehaviour monkeyMenBehaviour;
        public UIMerchantWaitTimer uiMerchantWaitTimer;
        private UIMerchant uiMerchant;

        public void Start() {
            monkeyMenBehaviour = GetComponent<MonkeyMenBehaviour>();
            uiMerchant = ObjectsReference.Instance.uiMerchant;
            ObjectsReference.Instance.chimpManager.merchimpsManager.activeMerchimpBehaviour = this;
            
            uiMerchant.InitializeInventories(monkeyMenBehaviour.monkeyMenData);
            uiMerchant.RefreshMerchantInventories();
            uiMerchant.RefreshBitkongQuantities();
            uiMerchant.Switch_to_Sell_inventory();
            
            StartWaitingTimer();
        }
        
        private int waitTimer;
        
        private void StartWaitingTimer() {
            transform.position = monkeyMenBehaviour.associatedSpaceship.transform.position;
            
            uiMerchantWaitTimer.SetTimer(120);
            waitTimer = 120;
            InvokeRepeating(nameof(DecrementeTimer), 0, 1);
        }
        
        public void DecrementeTimer() {
            waitTimer--;
            if (waitTimer <= 0) {
                transform.position = monkeyMenBehaviour.associatedSpaceship.transform.position;
                CancelInvoke(nameof(DecrementeTimer));
                monkeyMenBehaviour.associatedSpaceship.StopWaiting();
            }
            
            uiMerchantWaitTimer.SetTimer(waitTimer);
        }
    }
}