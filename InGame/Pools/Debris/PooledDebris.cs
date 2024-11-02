using UnityEngine;

namespace InGame.Pools.Debris {
    public class PooledDebris : MonoBehaviour {
        public int prefabIndex;
        
        public DebrisPool debrisPool;
        
       public void Release() {
           debrisPool.ReturnToPool(gameObject);
       }
    }
}
