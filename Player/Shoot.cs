using Particles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class Shoot : MonoBehaviour {
        public void shoot(InputAction.CallbackContext context) {
            if (context.started) {
                BananaMan.Instance.isShooting = true;
            }
        
            if (context.canceled) {
                BananaParticuleSystemManager.Instance.ActiveEmissionModule.enabled = false;
                BananaMan.Instance.isShooting = false;
            }
        }
    }
}
