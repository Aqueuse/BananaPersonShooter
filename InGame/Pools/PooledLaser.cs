using UnityEngine;

namespace InGame.Pools {
    public class PooledLaser : MonoBehaviour {
        [HideInInspector] public LaserPool pool;
        
        public void Release() {
           pool.ReturnToPool(gameObject);
       }
    }
}
