using Game.BananaCannonMiniGame;
using Game.CommandRoomPanelControls;
using Game.VisitorReceptionMiniGame;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class MarketingLaunchAdInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            VisitorReception.Instance.AddVisitorInWaitingLine(VisitorType.VISITOR_ADULT_BIG);
            
            //CommandRoomControlPanelsManager.Instance.marketingCampaignManager.StartCampaign();
        }
    }
}