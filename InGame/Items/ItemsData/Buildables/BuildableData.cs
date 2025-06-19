using System.Collections.Generic;
using UnityEngine;

namespace InGame.Items.ItemsData.Buildables {
    public class BuildableData: MonoBehaviour {
        public string buildableGuid;
        public BuildableState buildableState;
        public Dictionary<RawMaterialType, int> buildingMaterials = new ();
    }
}
