using Enums;
using Player;
using UI;
using UI.InGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Building {
    public class Mover : MonoSingleton<Mover> {
        [SerializeField] private GameObject deplaceur;
        [SerializeField] private GameObject moverTarget;
        
        [SerializeField] private UIRocketsQuantity uiRocketsQuantity;

        [SerializeField] private GameObject plateformSpawnerPoint;
        [SerializeField] private GenericDictionary<ItemThrowableType, GameObject> plateformsPrefab;

        public bool wasFocus;

        private PlayerController _playerController;

        public GameObject activePlateform;

        public bool isPlateformPlaced;

        private void Start() {
            _playerController = BananaMan.Instance.GetComponent<PlayerController>();
        }

        void Update() {
            if (BananaMan.Instance.isGrabingMover) {
                deplaceur.transform.LookAt(moverTarget.transform, Vector3.up);
                
                if (Physics.Raycast(deplaceur.transform.position, deplaceur.transform.forward, out RaycastHit raycastHit)) {
                    if (raycastHit.transform.tag.Equals("aspirable")) {
                        Destroy(raycastHit.transform.gameObject);
                        AddRocket();
                    }
                    
                    if (BananaMan.Instance.activeItemThrowableCategory == ItemThrowableCategory.PLATEFORM && activePlateform != null) {
                        Ray ray = new Ray(deplaceur.transform.position, deplaceur.transform.forward);
                        activePlateform.transform.position = ray.GetPoint(15);
                    }
                }
            }
        }
        
        public void Acquire() {
            BananaMan.Instance.hasMover = true;
            PlayerPrefs.SetString("HasMover", "true");
        }

        public void Activate(InputAction.CallbackContext context) {
            if (context.performed) {
                deplaceur.SetActive(true);
                BananaMan.Instance.tpsPlayerAnimator.GrabMover();
                BananaMan.Instance.isGrabingMover = true;
                UIManager.Instance.Show_Hide_Rocks_Quantity(true);

                UICrosshair.Instance.SetCrosshair(ItemThrowableType.PLATEFORM_CAVENDISH, ItemThrowableCategory.PLATEFORM);

                if (BananaMan.Instance.activeItemThrowableCategory == ItemThrowableCategory.PLATEFORM) {
                    var plateformType = UISlotsManager.Instance.Get_Selected_Slot().itemThrowableType;

                    if (Inventory.Instance.bananaManInventory[plateformType] > 0) {
                        activePlateform = Instantiate(original:plateformsPrefab[BananaMan.Instance.activeItemThrowableType],
                            position: plateformSpawnerPoint.transform.position,
                            rotation: plateformSpawnerPoint.transform.localRotation);
                    }
                }

                wasFocus = _playerController.isFocusCamera;

            }

            if (context.canceled) {
                if (UISlotsManager.Instance.Get_Selected_Slot().itemThrowableCategory == ItemThrowableCategory.PLATEFORM) {
                    var plateformType = UISlotsManager.Instance.Get_Selected_Slot().itemThrowableType;
                    
                    if (activePlateform.GetComponent<Plateform>().isValid && Inventory.Instance.bananaManInventory[plateformType] > 0) {
                        activePlateform.GetComponent<Plateform>().SetNormal();

                        Inventory.Instance.RemoveQuantity(plateformType, 1);
                        UISlotsManager.Instance.Get_Selected_Slot().SetAmmoQuantity(Inventory.Instance.GetQuantity(plateformType).ToString());
                    }
                    else {
                        Destroy(activePlateform);
                    }
                }

                deplaceur.SetActive(false); 
                BananaMan.Instance.isGrabingMover = false;
                UIManager.Instance.Show_Hide_Rocks_Quantity(false);
                UICrosshair.Instance.SetCrosshair(BananaMan.Instance.activeItemThrowableType, BananaMan.Instance.activeItemThrowableCategory);
                
                BananaMan.Instance.tpsPlayerAnimator.FocusCamera(wasFocus);
            }
        }
        
        public void AddRocket() {
            Inventory.Instance.bananaManInventory[ItemThrowableType.ROCKET] += 1;
            uiRocketsQuantity.Set_Rockets_Quantity(Inventory.Instance.bananaManInventory[ItemThrowableType.ROCKET]);
        }

        public void SetRocketsQuantity(int quantity) {
            uiRocketsQuantity.Set_Rockets_Quantity(quantity);
        }
    }
}
