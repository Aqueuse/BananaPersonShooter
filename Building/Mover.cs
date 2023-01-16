using Enums;
using Player;
using UI.InGame;
using UnityEngine;

namespace Building {
    public enum MoverContext {
        GET,
        PUT
    }

    public class Mover : MonoSingleton<Mover> {
        [SerializeField] private UIMover uiMover;

        public bool wasFocus;
    
        public MoverContext moverContext;
    
        private void Start() {
            moverContext = MoverContext.GET;
            BananaMan.Instance.isGrabingMover = false;
        }
        
        /// HELPERS
        public void Acquire() {
            BananaMan.Instance.hasMover = true;
            PlayerPrefs.SetString("HasMover", "true");
        }

        public void SwitchMoverContextUI(MoverContext newMoverContext) {
            uiMover.SwitchGetPut(newMoverContext);
        }

        public void AmmoReduce() {
            BananasDataScriptableObject activeWeaponData = BananaMan.Instance.activeItem;

            switch (activeWeaponData.bananaEffect) {
                case BananaEffect.TWO_SPLIT:
                    Inventory.Instance.RemoveQuantity(activeWeaponData.itemThrowableType, 2);
                    break;
                case BananaEffect.FIVE_SPLIT:
                    Inventory.Instance.RemoveQuantity(activeWeaponData.itemThrowableType, 5);
                    break;
                default:
                    Inventory.Instance.RemoveQuantity(activeWeaponData.itemThrowableType, 1);
                    break;
            }

            var newAmmoQuantity = Inventory.Instance.bananaManInventory[activeWeaponData.itemThrowableType];

            UISlotsManager.Instance.Get_Selected_Slot().SetAmmoQuantity(newAmmoQuantity);
        }
    }
}