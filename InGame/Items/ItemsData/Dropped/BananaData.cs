using System;

namespace InGame.Items.ItemsData.Dropped {
    [Serializable]
    public class BananaData {
        public string droppedGuid;
        public string droppedPosition;
        public string droppedRotation;
        public BananaType bananaType;
    }
}
