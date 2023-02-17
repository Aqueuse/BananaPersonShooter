using Cinemachine;
using UnityEngine;
using UnityEngine.Video;

namespace Game {
    public class Cinematiques : MonoSingleton<Cinematiques> {
        [SerializeField] private GenericDictionary<CinematiqueType, VideoClip> cinematiquesVideoClipsByType;

        [SerializeField] private Material videoMaterial;
        [SerializeField] private Material transparentVideoMaterial;

        private VideoPlayer cinematiqueVideoPlayer;
        private MeshRenderer meshRenderer;
        private CinemachineVirtualCamera cinematiqueCamera;

        private CinematiqueType cinematiqueType;
        
        private void Start() {
            cinematiqueVideoPlayer = GetComponentInChildren<VideoPlayer>();
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            cinematiqueCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            
            RenderTexture.active = cinematiqueVideoPlayer.targetTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = null;

            cinematiqueVideoPlayer.loopPointReached += EndReached;
        }

        public void Play(CinematiqueType playedCinematiqueType) {
            cinematiqueType = playedCinematiqueType;
            cinematiqueCamera.Priority = 100;
            meshRenderer.material = playedCinematiqueType == CinematiqueType.DEATH ? transparentVideoMaterial : videoMaterial;

            cinematiqueVideoPlayer.clip = cinematiquesVideoClipsByType[playedCinematiqueType];
            cinematiqueVideoPlayer.frame = 0;
            cinematiqueVideoPlayer.Play();
        }

        void EndReached(VideoPlayer videoPlayer) {
            cinematiqueCamera.Priority = 3;

            if (cinematiqueType == CinematiqueType.NEW_GAME) {
                GameManager.Instance.Start_New_Game();
            }
        }
    }
}
