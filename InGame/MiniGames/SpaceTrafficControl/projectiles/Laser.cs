using InGame.Pools;
using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControl.projectiles {
    public class Laser : MonoBehaviour {
        [SerializeField] private MeshRenderer projectileRenderer;

        private float shootingTime;
        
        private void Update() {
            transform.position += transform.forward * 10;

            shootingTime += 1;
            
            if (shootingTime > 1000) {
                GetComponent<PooledLaser>().Release();
                enabled = false;
            }
        }
    }
}