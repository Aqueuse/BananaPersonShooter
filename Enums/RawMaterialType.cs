using UnityEngine;

namespace Enums {
    public class RawMaterialDescriptor : MonoBehaviour {
        public RawMaterialType rawMaterialType;
    }

    public enum RawMaterialType {
        METAL,
        BATTERY,
        ELECTRONIC,
        FABRIC,
        BANANA_PEEL,
        EMPTY
    }
}