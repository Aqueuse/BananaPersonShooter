using System;

namespace InGame.Items.ItemsData.Dropped {
    [Serializable]
    public class ManufacturedItemData {
        public string droppedGuid;
        public string droppedPosition;
        public string droppedRotation;
        public ManufacturedItemsType manufacturedItemsType;
    }
}
