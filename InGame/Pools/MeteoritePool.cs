using UnityEngine;

namespace InGame.Pools {
    public class MeteoritePool : MonoBehaviour {
        [SerializeField] private GameObject[] meteoriteList;

        private PooledMeteorite[] pooledMeteorite;
        private GameObject meteoriteInstance;

        public GameObject GetPooledMeteorite(MeteoriteType meteoriteType) {
            pooledMeteorite = GetComponentsInChildren<PooledMeteorite>();

            foreach (var debris in pooledMeteorite) {
                if (debris.meteoriteType == meteoriteType) {
                    debris.gameObject.SetActive(true);
                    return debris.gameObject;
                }
            }
            
            meteoriteInstance = Instantiate(
                ObjectsReference.Instance.meshReferenceScriptableObject.meteoritePrefabByMeteoriteType[meteoriteType], 
                transform
            );
            
            meteoriteInstance.GetComponent<PooledMeteorite>().meteoritePool = this;
            meteoriteInstance.GetComponent<PooledMeteorite>().meteoriteType = meteoriteType;

            return meteoriteInstance;
        }

        public void ReturnToPool(GameObject pooledObject) {
            pooledObject.transform.parent = transform;
            pooledObject.transform.position = Vector3.zero;
            pooledObject.transform.rotation = Quaternion.identity;

            pooledObject.SetActive(false);
        }
    }
}