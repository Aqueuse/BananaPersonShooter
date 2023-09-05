using Enums;
using UnityEngine;

namespace Game.Inventory {
    public class BlueprintsInventory : MonoBehaviour {
        public GenericDictionary<BuildableType, int> blueprintsInventory;
        
        public GameObject lastselectedInventoryItem;
    }
}