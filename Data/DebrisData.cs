using System;

namespace Data {
    [Serializable]
    public class DebrisData {
        public string debrisGuid;
        public string debrisPosition;
        public string debrisRotation;
        public int prefabIndex;
        public CharacterType characterType;
    }
}
