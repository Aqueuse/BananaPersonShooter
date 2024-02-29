using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class MarketingLaunchAdInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            //VisitorReception.Instance.AddVisitorInWaitingLine();
            //ChimpManager.Instance.SpawnPirate();
            
            ObjectsReference.Instance.spaceTrafficControlManager.SpawnSpaceshipBehaviour(CharacterType.MERCHIMP, 1);
            
            //CommandRoomControlPanelsManager.Instance.marketingCampaignManager.StartCampaign();
        }
    }
}