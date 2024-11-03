using System;

namespace InGame.Items.ItemsData {
    [Serializable]
    public class SpaceshipDebrisData {
        public string droppedGuid;
        public string spaceshipDebrisPosition;
        public string spaceshipDebrisRotation;
        public SpaceshipType spaceshipType;
        public int prefabIndex;

        public bool isInSpace;
        public BananaEffect bananaEffect;
        public string effectSourcePosition;
    }
}
