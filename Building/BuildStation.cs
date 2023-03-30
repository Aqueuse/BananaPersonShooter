using Building.Plateforms;
using Enums;
using Game;
using UI;
using UI.InGame;
using UnityEngine;

namespace Building {
    public class BuildStation : MonoBehaviour {
        [SerializeField] private UICanvasItemsStatic buildStationUiCanvasItemsStatic;
        [SerializeField] private GenericDictionary<ItemThrowableType, GameObject> grabbableItemsByType;
        
        private Animator _buildStationAnimator;
        private AudioSource _audioBuildstationSource;
        
        private GameObject _printedItemGameObject;
        private static readonly int PrintTrigger = Animator.StringToHash("PRINT");

        [HideInInspector] public ItemThrowableType activeItemType;
        [HideInInspector] public int quantityToPrint;
        [HideInInspector] public ItemThrowableType rawMaterial;

        [HideInInspector] public int totalCost;

        private void Start() {
            _buildStationAnimator = GetComponent<Animator>();
            _audioBuildstationSource = GetComponentInChildren<AudioSource>();
        }

        public void Print() {
            if (quantityToPrint > 0) {
                Inventory.Instance.RemoveQuantity(rawMaterial, totalCost);
                _buildStationAnimator.SetTrigger(PrintTrigger);
                _audioBuildstationSource.Play();

                UIManager.Instance.uiBuildStation.HideBuildStationInterface();
                buildStationUiCanvasItemsStatic.gameObject.layer = LayerMask.NameToLayer("Default");
                buildStationUiCanvasItemsStatic.HideUI();
            }
        }
        
        public void AddToStack() {
            if (_printedItemGameObject == null) {
                _printedItemGameObject = Instantiate(grabbableItemsByType[activeItemType]);
            }

            _printedItemGameObject.GetComponent<GrabbableItem>().AddQuantity(1);
            quantityToPrint--;
            
            if (quantityToPrint > 0) {
                _buildStationAnimator.SetTrigger(PrintTrigger);
            }
            else {
                _audioBuildstationSource.Stop();
            }
        }

        public void RemovePlatform() {
            Destroy(_printedItemGameObject);
            buildStationUiCanvasItemsStatic.gameObject.layer = LayerMask.NameToLayer("Items");
        }

    }
}
