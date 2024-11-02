using TMPro;
using UnityEngine;

namespace UI.InGame.MiniChimpView {
    public class UIStats : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI visitorsNumberText;
        [SerializeField] private TextMeshProUGUI piratesNumberText;
        [SerializeField] private TextMeshProUGUI merchantsNumberText;
        
        [SerializeField] private TextMeshProUGUI metalNumber;
        [SerializeField] private TextMeshProUGUI electronicPartsNumber;
        [SerializeField] private TextMeshProUGUI batteriesNumber;
        [SerializeField] private TextMeshProUGUI fabricNumber;
        [SerializeField] private TextMeshProUGUI bananaPeelsNumber;

        private int merchimpsNumber;
        private int touristesNumber;
        private int piratesNumber;
        
        public void RefreshStats() {
            merchimpsNumber = 0;
            touristesNumber = 0;
            piratesNumber = 0;
            
            foreach (var spaceshipBehaviour in ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid) {
                if (spaceshipBehaviour.Value.travelState == TravelState.WAIT_IN_STATION) {
                    if (spaceshipBehaviour.Value.characterType == CharacterType.MERCHIMP) {
                        merchimpsNumber += 1;
                    }

                    if (spaceshipBehaviour.Value.characterType == CharacterType.TOURIST) {
                        var touristSpaceship = spaceshipBehaviour.Value;
                        touristesNumber += touristSpaceship.touristDatas.Count;
                    }
                    
                    if (spaceshipBehaviour.Value.characterType == CharacterType.PIRATE) {
                        var pirateSpaceship = spaceshipBehaviour.Value;
                        piratesNumber += pirateSpaceship.monkeyMensData.Count;
                    }
                }

                piratesNumberText.text = piratesNumber.ToString();
                visitorsNumberText.text = touristesNumber.ToString();
                merchantsNumberText.text = merchimpsNumber.ToString();
            }
            
            metalNumber.text = ObjectsReference.Instance.droppedInventory.GetQuantity(DroppedType.METAL).ToString();
            electronicPartsNumber.text = ObjectsReference.Instance.droppedInventory.GetQuantity(DroppedType.ELECTRONIC).ToString();
            batteriesNumber.text = ObjectsReference.Instance.droppedInventory.GetQuantity(DroppedType.BATTERY).ToString();
            fabricNumber.text = ObjectsReference.Instance.droppedInventory.GetQuantity(DroppedType.FABRIC).ToString();
            bananaPeelsNumber.text = ObjectsReference.Instance.droppedInventory.GetQuantity(DroppedType.BANANA_PEEL).ToString();
        }
    }
}
