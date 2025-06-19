using System;
using System.Collections.Generic;

namespace Save.Buildables {
    [Serializable]
    public class BuildableSavedData {
        public string buildableGuid;
        public BuildableType buildableType;
        public bool isBreaked;
        public string buildablePosition;
        public string buildableRotation;

        public BuildableState buildableState;
        public Dictionary<RawMaterialType, int> buildingMaterials;
    }
}
