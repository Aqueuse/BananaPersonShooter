using System;
using System.Collections.Generic;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;

namespace InGame.Items.ItemsData.BuildablesData {
    [Serializable]
    public class BananasDryerData : BuildableData {
        public List<BananaDryerSlot> bananaDryerSlots;
    }
}
