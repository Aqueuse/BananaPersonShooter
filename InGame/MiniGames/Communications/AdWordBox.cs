using UnityEngine;

namespace InGame.MiniGames.Communications {
    public class AdWordBox : MonoBehaviour {
        public int touristsWeight;
        public int piratesWeight;
        public int merchantsWeight;

        public bool isInCurrentCampaign;

        public void Activate() {
            if (isInCurrentCampaign) {
                ObjectsReference.Instance.communicationsManager.adCampaign.RemoveAdWordBox(this);
                isInCurrentCampaign = false;
            }
            else {
                ObjectsReference.Instance.communicationsManager.adCampaign.TryAddWordToCampaign(this);
            }
        }
    }
}
