using UnityEngine;

namespace InGame.Pools {
    public class PooledMeteorite : MonoBehaviour {
        public MeteoriteType meteoriteType;
        
        public MeteoritePool meteoritePool;
        
       public void Release() {
           meteoritePool.ReturnToPool(gameObject);
       }
    }
}
