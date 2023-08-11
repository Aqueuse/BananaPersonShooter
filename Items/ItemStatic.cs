using Building.Buildables;
using UI.InGame;
using UnityEngine;

namespace Items {
    public class ItemStatic : MonoBehaviour {
        [SerializeField] private ItemInteraction itemInteraction;
        
        public ItemStaticType itemStaticType;

        private bool _isActive;

        public void Activate() {
            if (_isActive) return;

            itemInteraction.ShowUI();
            _isActive = true;

            if (itemStaticType == ItemStaticType.BANANA_DRYER_PLACE_BANANA_PEEL) {
                ObjectsReference.Instance.inputManager.bananasDryerAction.enabled = true;
                ObjectsReference.Instance.inputManager.bananasDryerAction.activeBananaDryer = GetComponent<BananasDryer>();
            }
        }

        public void Desactivate() {
            _isActive = false;

            itemInteraction.HideUI();

            if (itemStaticType == ItemStaticType.BANANA_DRYER_PLACE_BANANA_PEEL) {
                ObjectsReference.Instance.inputManager.bananasDryerAction.enabled = false;
                ObjectsReference.Instance.inputManager.bananasDryerAction.activeBananaDryer = null;
            }
        }
    }
}
