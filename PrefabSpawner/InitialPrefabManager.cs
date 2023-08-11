using Building.Buildables.Plateforms;
using UnityEngine;

namespace PrefabSpawner {
    public class InitialPrefabManager : MonoBehaviour {
        [SerializeField] private Plateform[] plateformes;
        [SerializeField] private ItemType plateformType;

        private void Start() {
            if (!ObjectsReference.Instance.mapsManager.currentMap.isDiscovered) {
                foreach (var plateforme in plateformes) {
                    plateforme.ActivePlateform(plateformType);
                }

                ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;
                ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesItemsDataMap();
            }
        }
    }
}

