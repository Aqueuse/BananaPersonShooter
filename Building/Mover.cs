using Enums;
using TMPro;
using UI;
using UI.InGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class Mover : MonoSingleton<Mover> {
        [SerializeField] private GameObject deplaceur;
        [SerializeField] private GameObject moverTarget;

        [SerializeField] private UIRocketsQuantity _uiRocketsQuantity;
        
        public bool wasFocus;

        private PlayerController _playerController;

        private void Start() {
            _playerController = BananaMan.Instance.GetComponent<PlayerController>();
        }

        void Update() {
            if (BananaMan.Instance.isGrabingMover) {
                deplaceur.transform.LookAt(moverTarget.transform, Vector3.up);
                
                if (Physics.Raycast(deplaceur.transform.position, deplaceur.transform.forward,
                        out RaycastHit raycastHit)) {
                    if (raycastHit.transform.tag.Equals("aspirable")) {
                        Destroy(raycastHit.transform.gameObject);
                        AddRocket();
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
                
                wasFocus = _playerController.isFocusCamera;
                
                
            }

            if (context.canceled) {
                deplaceur.SetActive(false); 
                BananaMan.Instance.isGrabingMover = false;
                UIManager.Instance.Show_Hide_Rocks_Quantity(false);
                
                BananaMan.Instance.tpsPlayerAnimator.FocusCamera(wasFocus);
            }
        }

        public void Move() {
            if (BananaMan.Instance)
                // if plateformer selected => put plateform on environnement
        
                // else if camera target == caillou => destroy caillou and take it in the inventory
                Debug.Log("coin");
        }

        public void AddRocket() {
            Inventory.Instance.bananaManInventory[ItemThrowableType.ROCKET] += 1;
            _uiRocketsQuantity.Set_Rockets_Quantity(Inventory.Instance.bananaManInventory[ItemThrowableType.ROCKET]);
        }

        public void SetRocketsQuantity(int quantity) {
            _uiRocketsQuantity.Set_Rockets_Quantity(quantity);
        }
    }
}
