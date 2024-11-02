using System;

namespace InGame.Items.ItemsData {
    [Serializable]
    public class SpaceshipDebrisData {
        public string droppedGuid;
        public string spaceshipDebrisPosition;
        public string spaceshipDebrisRotation;
        public int prefabIndex;
        public CharacterType characterType;

        public bool isInSpace;
    }
}
