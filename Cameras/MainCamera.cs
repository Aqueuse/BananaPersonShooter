using Cinemachine;
using UnityEngine;

namespace Cameras {
    public class MainCamera : MonoSingleton<MainCamera> {
        public GameObject bananaSplashVideo;

        [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
        [SerializeField] private CinemachineFreeLook cinemachineShootFreeLook;

        public void Switch_To_TPS_Target() {
            cinemachineFreeLook.enabled = true;
            cinemachineShootFreeLook.enabled = false;
        }

        public void Switch_To_Shoot_Target() {
            cinemachineFreeLook.enabled = false;
            cinemachineShootFreeLook.enabled = true;
        }
    }
}
