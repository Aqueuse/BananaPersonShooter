using InGame.Player.PlayerActions;
using UnityEngine;

namespace InGame.Player {
    public class PlayerActionsSwitch : MonoBehaviour {
        public PlayerActionsType playerActions;
        
        public Build build;
        public Scan scan;

        public void SwitchToPlayerAction(PlayerActionsType playerActionsType) {
            playerActions = playerActionsType;

            switch (playerActionsType) {
                case PlayerActionsType.BUILD:
                    build.enabled = true;
                    scan.enabled = false;
                    break;
                case PlayerActionsType.SCAN:
                    build.enabled = false;
                    scan.enabled = true;
                    break;
                case PlayerActionsType.THROW_BANANA or PlayerActionsType.IDLE:
                    build.enabled = false;
                    scan.enabled = false;
                    break;
            }
        }
    }
}
