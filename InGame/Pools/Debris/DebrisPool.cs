using UnityEngine;

namespace InGame.Pools.Debris {
    public class DebrisPool : MonoBehaviour {
        [SerializeField] private GameObject[] debrisList;

        private PooledDebris[] pooledDebris;
        private GameObject debrisInstance;

        public GameObject GetPooledDebris(int index) {
            pooledDebris = GetComponentsInChildren<PooledDebris>();

            foreach (var debris in pooledDebris) {
                if (debris.prefabIndex == index) {
                    debris.gameObject.SetActive(true);
                    return debris.gameObject;
                }
            }
            
            debrisInstance = Instantiate(debrisList[index], transform);
            debrisInstance.GetComponent<PooledDebris>().debrisPool = this;
            debrisInstance.GetComponent<PooledDebris>().prefabIndex = index;

            return debrisInstance;
        }

        public void ReturnToPool(GameObject pooledObject) {
            pooledObject.transform.parent = transform;
            pooledObject.transform.position = Vector3.zero;
            pooledObject.transform.rotation = Quaternion.identity;

            pooledObject.SetActive(false);
        }
    }
}