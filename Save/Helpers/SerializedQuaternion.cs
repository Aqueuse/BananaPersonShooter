using System;

namespace Save.Helpers {
    [Serializable]
    public class SerializedQuaternion {
        public float x;
        public float y;
        public float z;
        public float w;

        public SerializedQuaternion(float x, float y, float z, float w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
}