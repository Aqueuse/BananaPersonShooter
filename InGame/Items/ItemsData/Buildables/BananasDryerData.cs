using System.Collections.Generic;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using Save.Buildables;

namespace InGame.Items.ItemsData.Buildables {
    public class BananasDryerData : BuildableSavedData {
        public List<BananaDryerSlot> bananaDryerSlots;
        public Dictionary<RawMaterialType, int> materialsInSlots;
    }
}
