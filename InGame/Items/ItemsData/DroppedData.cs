using System;

namespace InGame.Items.ItemsData {
    [Serializable]
    public class DroppedData {
        public string droppedGuid;
        public string droppedPosition;
        public string droppedRotation;
        public DroppedType droppedType;
    }
}
