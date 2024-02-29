using UnityEngine;
using UnityEngine.Pool;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles {
    public class ProjectilesPool : MonoBehaviour {
        public Projectile projectilePrefab;
        private ObjectPool<Projectile> _projectilesPool;

        private void Start() {
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
            var projectile = Instantiate(projectilePrefab, transform);
            projectile.SetPool(_projectilesPool);
            return projectile;
        }

        private static void OnObject(Projectile projectile) {
            projectile.gameObject.SetActive(true);
        }

        private static void OnReleased(Projectile projectile) {
            projectile.gameObject.SetActive(false);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        private static void OnDestroyPoolObject(Projectile projectile) {
            Destroy(projectile.gameObject);
        }

        public Projectile Get_projectile() {
            return _projectilesPool.Get();
        }
    }
}