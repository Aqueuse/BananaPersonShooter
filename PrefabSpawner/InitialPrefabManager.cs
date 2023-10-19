using Gestion.Buildables.Plateforms;
using Data.Bananas;
using UnityEngine;

namespace PrefabSpawner {
    public class InitialPrefabManager : MonoBehaviour {
        [SerializeField] private Plateform[] plateformes;
        [SerializeField] private BananasDataScriptableObject bananasDataScriptableObject;

        private void Start() {
            if (!ObjectsReference.Instance.mapsManager.currentMap.isDiscovered) {
                foreach (var plateforme in plateformes) {
                    plateforme.ActivePlateform(bananasDataScriptableObject);
                }

                ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;
                ObjectsReference.Instance.mapsManager.currentMap.RefreshItemsDataMap();
            }
        }
    }
}

