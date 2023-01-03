using Enums;
using Player;
using UI;
using UI.InGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Building {
    public enum MoverContext {
        GET,
        PUT
    }

    public class Mover : MonoSingleton<Mover> {
        [SerializeField] private GameObject deplaceur;
        [SerializeField] private GameObject moverTarget;
        
        [SerializeField] private GameObject plateformSpawnerPoint;
        [SerializeField] private GameObject launchingBananaPoint;

        [SerializeField] private UIMover uiMover;

        [SerializeField] private bool wasFocus;

        [SerializeField] private GenericDictionary<ItemThrowableType, GameObject> plateformsPrefab;
        [SerializeField] private GenericDictionary<ItemThrowableType, GameObject> weaponsGameObjects;

        private GameObject activePlateform;
        private PlayerController _playerController;
        private MoverContext _moverContext;

        private void Start() {
            _playerController = BananaMan.Instance.GetComponent<PlayerController>();
            _moverContext = MoverContext.GET;

            BananaMan.Instance.isGrabingMover = false;
        }

        void Update() {
            if (BananaMan.Instance.isGrabingMover) {
                deplaceur.transform.LookAt(moverTarget.transform, Vector3.up);

                if (_moverContext == MoverContext.PUT) {
                    if (BananaMan.Instance.activeItemThrowableCategory == ItemThrowableCategory.PLATEFORM && activePlateform != null) {
                        Ray ray = new Ray(deplaceur.transform.position, deplaceur.transform.forward);
                        activePlateform.transform.position = ray.GetPoint(15);
                    }
                }

                if (_moverContext == MoverContext.GET) {
                    if (Physics.Raycast(deplaceur.transform.position, deplaceur.transform.forward, out RaycastHit raycastHit)) {
                        // GET ROCKET
                        if (raycastHit.transform.tag.Equals("aspirable")) {
                            AddRocket();
                            Destroy(raycastHit.transform.gameObject);
                        }
                    
                        // GET PLATEFORM
                        if (raycastHit.transform.GetComponent<Plateform>() != null) {
                            Inventory.Instance.AddQuantity(raycastHit.transform.GetComponent<Plateform>().platformType, 1);
                            Destroy(raycastHit.transform.gameObject);
                        }
                    }
                }
            }
        }


        public void switch_Get_Put(InputAction.CallbackContext context) {
            if (_moverContext == MoverContext.GET) {
                _moverContext = MoverContext.PUT;
                uiMover.SwitchGetPut(MoverContext.PUT);
            }
            else {
                _moverContext = MoverContext.GET;
                uiMover.SwitchGetPut(MoverContext.GET);
            }
            
        }
        
        public void Move(InputAction.CallbackContext context) {
            var lastSelectedThrowableItem = BananaMan.Instance.activeItemThrowableType;
            var lastSelectedThrowableCategory = BananaMan.Instance.activeItemThrowableCategory;

            UICrosshair.Instance.SetCrosshair(ItemThrowableType.PLATEFORM_CAVENDISH, ItemThrowableCategory.PLATEFORM);
            UIManager.Instance.Show_Hide_Mover_UI(true);

            if (_moverContext == MoverContext.GET) {
                if (context.performed && GameManager.Instance.isGamePlaying) {
                    BananaMan.Instance.isGrabingMover = true;
                    
                    deplaceur.SetActive(true);
                    BananaMan.Instance.tpsPlayerAnimator.GrabMover();
                    BananaMan.Instance.isGrabingMover = true;
                    
                    wasFocus = _playerController.isFocusCamera;
                }
                
                if (context.canceled) {
                    BananaMan.Instance.isGrabingMover = false;
                    deplaceur.SetActive(false);

                    UICrosshair.Instance.SetCrosshair(BananaMan.Instance.activeItemThrowableType, BananaMan.Instance.activeItemThrowableCategory);
                    UIManager.Instance.Show_Hide_Mover_UI(false);

                    BananaMan.Instance.tpsPlayerAnimator.FocusCamera(wasFocus);
                }
            }

            if (_moverContext == MoverContext.PUT) {
                if (context.performed) {
                    deplaceur.SetActive(true);
                    BananaMan.Instance.tpsPlayerAnimator.GrabMover();
                    BananaMan.Instance.isGrabingMover = true;
                    
                    wasFocus = _playerController.isFocusCamera;

                    // PUT PLATEFORMS
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

                if (context.canceled) {
                    BananaMan.Instance.isGrabingMover = false;
                    UICrosshair.Instance.SetCrosshair(BananaMan.Instance.activeItemThrowableType, BananaMan.Instance.activeItemThrowableCategory);
                    UIManager.Instance.Show_Hide_Mover_UI(false);

                    deplaceur.SetActive(false);
                    BananaMan.Instance.tpsPlayerAnimator.FocusCamera(wasFocus);

                    // PUT PLATEFORMS (validate)
                    if (lastSelectedThrowableCategory == ItemThrowableCategory.PLATEFORM) {
                        if (activePlateform != null) {
                            if (activePlateform.GetComponent<Plateform>().isValid && Inventory.Instance.bananaManInventory[lastSelectedThrowableItem] > 0) {
                                activePlateform.GetComponent<Plateform>().SetNormal();

                                Inventory.Instance.RemoveQuantity(lastSelectedThrowableItem, 1);
                                UISlotsManager.Instance.Get_Selected_Slot()
                                    .SetAmmoQuantity(Inventory.Instance.GetQuantity(lastSelectedThrowableItem).ToString());
                            }
                            else {
                                Destroy(activePlateform);
                            }
                        }
                    }

                }
            }
        }

        // PUT BANANAS
        public void ThrowBanana(InputAction.CallbackContext context) {
            if (context.performed && GameManager.Instance.isGamePlaying) {
                if (_moverContext == MoverContext.PUT && BananaMan.Instance.isGrabingMover && Inventory.Instance.GetQuantity(BananaMan.Instance.activeItemThrowableType) > 0) {
                    var banana = Instantiate(weaponsGameObjects[BananaMan.Instance.activeItem.itemThrowableType], null);

                    // Instantiate Banana of this type
                    // throw it with good speed forward the player
                    banana.transform.position = launchingBananaPoint.transform.position;
                    banana.GetComponent<Rigidbody>().AddForce(BananaMan.Instance.transform.forward * 50, ForceMode.Impulse);
                    AmmoReduce();
                } 
            }
        }

        /// <summary>
        /// HELPERS
        /// </summary>
        public void AddRocket() {
            Inventory.Instance.bananaManInventory[ItemThrowableType.ROCKET] += 1;
        }
        
        public void Acquire() {
            BananaMan.Instance.hasMover = true;
            PlayerPrefs.SetString("HasMover", "true");
        }


        void AmmoReduce() {
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

            UISlotsManager.Instance.Get_Selected_Slot().SetAmmoQuantity(newAmmoQuantity.ToString());
        }
    }
}