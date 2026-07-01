using System.Collections.Generic;
using System.Linq;
using InGame.MiniGames.SpaceTrafficControl.projectiles;
using UnityEngine;

namespace InGame.Pools {
    public class LaserPool : MonoBehaviour {
        [SerializeField] private GameObject laserPrefab;

        private List<PooledLaser> pooledLaser;
        private GameObject laserInstance;

        public GameObject GetPooledLaser() {
            pooledLaser = GetComponentsInChildren<PooledLaser>().ToList();

            foreach (var laser in pooledLaser) {
                if (!laser.GetComponent<Laser>().enabled) {
                    laser.gameObject.SetActive(true);
                    return laser.gameObject;
                }
            }
            
            laserInstance = Instantiate(
                laserPrefab, 
                transform
            );
            
            pooledLaser.Add(laserInstance.GetComponent<PooledLaser>());
            
            laserInstance.GetComponent<PooledLaser>().pool = this;

            return laserInstance;
        }

        public void ReturnToPool(GameObject pooledObject) {
            pooledObject.transform.parent = transform;
            pooledObject.transform.position = Vector3.zero;
            pooledObject.transform.rotation = Quaternion.identity;

            pooledObject.SetActive(false);
        }
    }
}