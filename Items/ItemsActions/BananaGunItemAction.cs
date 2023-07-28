using Enums;
using Game.CommandRoomPanelControls;
using Game.Steam;
using UnityEngine;

namespace Items.ItemsActions {
    public class BananaGunItemAction : MonoBehaviour {
        public static void Activate(GameObject _interactedObject) {
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.HUD, true);
            ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(true);
            // TODO : animation take banana gun

            ObjectsReference.Instance.steamIntegration.UnlockAchievement(SteamAchievement.STEAM_ACHIEVEMENT_BANAGUN_RECONSTRUCTED);
            ObjectsReference.Instance.bananaMan.hasRepairedBananaGun = true;
                        
            CommandRoomControlPanelsManager.Instance.SetMiniChimpDialogue(miniChimpDialogue.ASPIRE_CHIMPLOYEE);
            CommandRoomControlPanelsManager.Instance.commandRoomMiniChimpDialogue.Play();

            _interactedObject.SetActive(false);
        }
    }
}
