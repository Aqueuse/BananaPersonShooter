using Game.CommandRoomPanelControls;
using UnityEngine;

namespace Monkeys.MiniChimps {
    public class MiniChimpCommandRoom : MonoBehaviour {
        public GameObject TpButton;
        
        private int dialogueIndex;
        public bool dialogueShown;
        
        public static void AuthorizeDoorsAccess() {
            if (CommandRoomControlPanelsManager.Instance != null) CommandRoomControlPanelsManager.Instance.AuthorizeDoorsAccess();
        }
    }
}
