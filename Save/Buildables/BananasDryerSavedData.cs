using System;
using System.Collections.Generic;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;

namespace Save.Buildables {
    [Serializable]
    public class BananasDryerSavedData : BuildableSavedData {
        public List<BananaDryerSlot> bananaDryerSlots;
        public Dictionary<RawMaterialType, int> materialsInSlots;
    }
}
