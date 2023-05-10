using UnityEngine;
using UnityEngine.Pool;

namespace Game.BananaCannonMiniGame {
    public class ProjectilesPool : MonoBehaviour {
        public Projectile projectilePrefab;
        private ObjectPool<Projectile> _projectilesPool;

        void Start() {
            _projectilesPool = new ObjectPool<Projectile>(
                InstantiateObject,
                actionOnGet: OnObject,
                actionOnRelease: OnReleased,
                actionOnDestroy: OnDestroyPoolObject,
                collectionCheck: true,
                defaultCapacity:10,
                maxSize:10
            );
        }

        private Projectile InstantiateObject() {
            Projectile projectile = Instantiate(projectilePrefab, transform);
            projectile.SetPool(_projectilesPool);
            return projectile;
        }

        void OnObject(Projectile projectile) {
            projectile.gameObject.SetActive(true);
        }

        void OnReleased(Projectile projectile) {
            projectile.gameObject.SetActive(false);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(Projectile projectile) {
            Destroy(projectile.gameObject);
        }

        public Projectile Get_projectile() {
            return _projectilesPool.Get();
        }
    }
}