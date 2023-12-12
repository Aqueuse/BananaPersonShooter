using System;

namespace Save.Helpers {
    [Serializable]
    public class SerializedVector3 {
        public float x;
        public float y;
        public float z;

        public SerializedVector3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}