using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Weapons {
    public class BananasManager : MonoSingleton<BananasManager> {
        [SerializeField] private List<BananaType> bananaTypes;
        [SerializeField] private List<GameObject> bananasPrefabs;
        [SerializeField] private List<BananasDataScriptableObject> bananasScriptablesObjects;

        public Dictionary<BananaType, GameObject> WeaponsGameObjects;
        public Dictionary<BananaType, BananasDataScriptableObject> WeaponsDataScriptableObjects;
        
        private void Start() {
            WeaponsGameObjects = new Dictionary<BananaType, GameObject>();
            WeaponsDataScriptableObjects = new Dictionary<BananaType, BananasDataScriptableObject>();

            for (var i=0; i<bananasPrefabs.Count; i++) {
                WeaponsGameObjects.Add(bananaTypes[i], bananasPrefabs[i]);
            }

            for (var i = 0; i < bananaTypes.Count; i++) {
                WeaponsDataScriptableObjects.Add(bananaTypes[i], bananasScriptablesObjects[i]);
            }
        }
    }
}
