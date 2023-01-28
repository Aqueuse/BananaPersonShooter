using Enums;
using UnityEngine;

namespace Building {
    public class Debris : MonoBehaviour {
        private static readonly int Activate = Shader.PropertyToID("_Activate");

        public ItemThrowableCategory itemThrowableCategory;
        public ItemThrowableType itemThrowableType;

        public int prefabIndex;

        public void ShakeMe(bool isShaking) {
            GetComponent<MeshRenderer>().materials[0].SetFloat(Activate, isShaking ? 1 : 0);
        }
    }
}
