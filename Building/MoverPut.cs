using Audio;
using Enums;
using Input;
using Player;
using UI;
using UI.InGame;
using UnityEngine;

namespace Building {
    public class MoverPut : MonoSingleton<MoverPut> {
        [SerializeField] private GameObject launchingBananaPoint;
        [SerializeField] private GameObject plateformSpawnerPoint;
        [SerializeField] private GameObject deplaceur;

        [SerializeField] private GameObject moverTarget;

        [SerializeField] private GenericDictionary<ItemThrowableType, GameObject> plateformsPrefab;
        [SerializeField] private GenericDictionary<ItemThrowableType, GameObject> weaponsGameObjects;

        private PlayerController _playerController;
        private GameObject activePlateform;

        private void Start() {
            _playerController = BananaMan.Instance.GetComponent<PlayerController>();
        }

        void Update() {
            if (BananaMan.Instance.isGrabingMover) {
                deplaceur.transform.LookAt(moverTarget.transform, Vector3.up);
            
                if (Mover.Instance.moverContext == MoverContext.PUT) {
                    if (BananaMan.Instance.activeItemThrowableCategory == ItemThrowableCategory.PLATEFORM && activePlateform != null) {
                        Ray ray = new Ray(deplaceur.transform.position, deplaceur.transform.forward);
                        activePlateform.transform.position = ray.GetPoint(15);
                    }
                }
            }
        }

        public void StartToPut() {
            deplaceur.SetActive(true);
            Mover.Instance.moverContext = MoverContext.PUT;
            BananaMan.Instance.isGrabingMover = true;

            var lastSelectedThrowableItem = BananaMan.Instance.activeItemThrowableType;
            var lastSelectedThrowableCategory = BananaMan.Instance.activeItemThrowableCategory;

            UICrosshair.Instance.SetCrosshair(ItemThrowableType.PLATEFORM_CAVENDISH, ItemThrowableCategory.PLATEFORM);
            UIManager.Instance.Show_Hide_Mover_UI(true);
            Mover.Instance.SwitchMoverContextUI(MoverContext.PUT);

            BananaMan.Instance.tpsPlayerAnimator.GrabMover();

            Mover.Instance.wasFocus = _playerController.isFocusCamera;

            if (lastSelectedThrowableCategory == ItemThrowableCategory.BANANA) {
                AudioManager.Instance.PlayEffect(EffectType.LOADING_GUN_PUT);
                Invoke(nameof(ThrowBanana), 1f);
            }
            
            if (lastSelectedThrowableCategory == ItemThrowableCategory.PLATEFORM) {
                if (Inventory.Instance.bananaManInventory[lastSelectedThrowableItem] > 0) {
                    deplaceur.SetActive(true);
                    BananaMan.Instance.tpsPlayerAnimator.GrabMover();

                    activePlateform = Instantiate(
                        original: plateformsPrefab[BananaMan.Instance.activeItemThrowableType],
                        position: plateformSpawnerPoint.transform.position,
                        rotation: plateformSpawnerPoint.transform.localRotation);
                }
            }
        }

        public void ValidatePut() {
            var lastSelectedThrowableItem = BananaMan.Instance.activeItemThrowableType;
            var lastSelectedThrowableCategory = BananaMan.Instance.activeItemThrowableCategory;

            BananaMan.Instance.isGrabingMover = false;
            UICrosshair.Instance.SetCrosshair(BananaMan.Instance.activeItemThrowableType,
                BananaMan.Instance.activeItemThrowableCategory);
            UIManager.Instance.Show_Hide_Mover_UI(false);

            deplaceur.SetActive(false);
            BananaMan.Instance.tpsPlayerAnimator.FocusCamera(Mover.Instance.wasFocus);
            
            if (lastSelectedThrowableCategory == ItemThrowableCategory.PLATEFORM) {
                if (activePlateform != null) {
                    if (activePlateform.GetComponent<Plateform>().isValid && Inventory.Instance.GetQuantity(lastSelectedThrowableItem) > 0) {
                        activePlateform.GetComponent<Plateform>().SetNormal();

                        Inventory.Instance.RemoveQuantity(lastSelectedThrowableItem, 1);
                        UISlotsManager.Instance.Get_Selected_Slot()
                            .SetAmmoQuantity(Inventory.Instance.GetQuantity(lastSelectedThrowableItem));
                    }
                    else {
                        Destroy(activePlateform);
                    }
                }
            }
        }

        public void ThrowBanana() {
            if (Inventory.Instance.GetQuantity(BananaMan.Instance.activeItemThrowableType) > 0) {
                if (GameActions.Instance.leftClickActivated || GameActions.Instance.rightTriggerActivated) {
                    var banana = Instantiate(weaponsGameObjects[BananaMan.Instance.activeItem.itemThrowableType],
                        launchingBananaPoint.transform.position, Quaternion.identity, null);

                    // Instantiate Banana of this type
                    banana.transform.SetParent(null);

                    // throw it with good speed forward the player
                    banana.GetComponent<Rigidbody>().AddForce(BananaMan.Instance.transform.forward * 100, ForceMode.Impulse);
                    AudioManager.Instance.PlayEffect(EffectType.THROW_BANANA);
                    Mover.Instance.AmmoReduce();
                }
            }
        }
    }
}