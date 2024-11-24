using System;

namespace InGame.Items.ItemsData.Dropped {
    [Serializable]
    public class RawMaterialData {
        public string droppedGuid;
        public string droppedPosition;
        public string droppedRotation;
        public RawMaterialType RawMaterialType;
    }
}
