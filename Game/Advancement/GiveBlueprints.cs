using Enums;
using UnityEngine;

namespace Game.Advancement {
    public class GiveBlueprints : MonoBehaviour {
        [SerializeField] private AdvancementState advancementState;

        public void Start() {
            ObjectsReference.Instance.advancements.TryAddBlueprintByAdvancementState(advancementState);
        }
    }
}
