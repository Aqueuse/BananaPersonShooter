using System;

namespace Data.BuildablesData {
    [Serializable]
    public class BuildableData {
        public string buildableGuid;
        public BuildableType buildableType;
        public string buildablePosition;
        public string buildableRotation;
    }
}
