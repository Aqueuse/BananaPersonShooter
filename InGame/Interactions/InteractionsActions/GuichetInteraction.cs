using InGame.MiniGames.Guichets;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class GuichetInteraction : Interaction {
        private Guichet guichet;
        
        public override void Activate(GameObject interactedGameObject) {
            guichet = interactedGameObject.transform.parent.GetComponent<Guichet>();
            
            ObjectsReference.Instance.uiGuichet.activatedGuichet = guichet;
            guichet.OpenGuichet();
        }
    }
}
