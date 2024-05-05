using Newtonsoft.Json;
using UnityEngine;

namespace Save.Helpers {
    public static class JsonHelper {
        public static Vector3 FromStringToVector3(string json) {
            var vector3 = JsonConvert.DeserializeObject<SerializedVector3>(json);
        
            return new Vector3(vector3.x, vector3.y, vector3.z);
        }

        public static string FromVector3ToString(Vector3 vector3) {
            var serializedVector3 = new SerializedVector3(x: vector3.x, y: vector3.y, z: vector3.z);
        
            return JsonConvert.SerializeObject(serializedVector3);
        }
    
        public static Quaternion FromStringToQuaternion(string json) {
            var quaternion = JsonConvert.DeserializeObject<SerializedQuaternion>(json);

            return new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }

        public static string FromQuaternionToString(Quaternion quaternion) {
            var serializedQuaternion = new SerializedQuaternion(x: quaternion.x, y: quaternion.y, z: quaternion.z, w: quaternion.w);

            return JsonConvert.SerializeObject(serializedQuaternion);
        }

        public static Color FromStringToColor(string color) {
            ColorUtility.TryParseHtmlString(color, out var colorToRetry);
            return colorToRetry;
        }

        public static string FromColorToString(Color color) {
            return "#"+ColorUtility.ToHtmlStringRGBA(color);
        }
    }
}