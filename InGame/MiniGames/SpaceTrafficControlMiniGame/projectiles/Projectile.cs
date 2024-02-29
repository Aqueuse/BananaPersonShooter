using UnityEngine;
using UnityEngine.Pool;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles {
    public class Projectile : MonoBehaviour {
        [SerializeField] private MeshRenderer projectileRenderer;
        private IObjectPool<Projectile> _objectPool;


        public void SetPool(IObjectPool<Projectile> pool) {
            _objectPool = pool;
        }

        public void SetColor(Color color) {
            projectileRenderer.material.color = color;
        }

        public void Destroy() {
            _objectPool.Release(this);
        }
    }
}