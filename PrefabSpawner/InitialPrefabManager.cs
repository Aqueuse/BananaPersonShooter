using Building.Buildables.Plateforms;
using UnityEngine;

namespace PrefabSpawner {
    public class InitialPrefabManager : MonoBehaviour {
        [SerializeField] private Plateform[] plateformes;
        [SerializeField] private ItemType plateformType;

        private void Start() {
            foreach (var plateforme in plateformes) {
                plateforme.ActivePlateform(plateformType);
            }
        }
    }
}

