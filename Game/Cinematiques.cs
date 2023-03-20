using Audio;
using Cinemachine;
using Enums;
using Input.UIActions;
using UnityEngine;
using UnityEngine.Video;

namespace Game {
    public class Cinematiques : MonoSingleton<Cinematiques> {
        [SerializeField] private GenericDictionary<CinematiqueType, VideoClip> cinematiquesVideoClipsByType;

        [SerializeField] private Material videoMaterial;
        [SerializeField] private Material transparentVideoMaterial;

        [SerializeField] private GameObject skipCinematiqueGameObject;
        
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
            cinematiqueVideoPlayer.enabled = true;
            skipCinematiqueGameObject.SetActive(true);

            UISchemaSwitcher.Instance.SwitchUISchema(UISchemaSwitchType.CINEMATIQUE);
            GameManager.Instance.gameContext = GameContext.IN_CINEMATIQUE;

            cinematiqueType = playedCinematiqueType;
            cinematiqueCamera.Priority = 100;
            meshRenderer.material = playedCinematiqueType == CinematiqueType.DEATH ? transparentVideoMaterial : videoMaterial;

            cinematiqueVideoPlayer.clip = cinematiquesVideoClipsByType[playedCinematiqueType];
            cinematiqueVideoPlayer.frame = 0;
            cinematiqueVideoPlayer.Play();
            
            AudioManager.Instance.StopAudioSource(AudioSourcesType.MUSIC);
        }

        public void Pause() {
			if (cinematiqueVideoPlayer.enabled)
				cinematiqueVideoPlayer.Pause();
        }

        public void Unpause() {
            if(cinematiqueVideoPlayer.enabled)
                cinematiqueVideoPlayer.Play();
            UISchemaSwitcher.Instance.SwitchUISchema(UISchemaSwitchType.CINEMATIQUE);
        }

        public void Skip() {
            cinematiqueCamera.Priority = 3;
            
            skipCinematiqueGameObject.SetActive(false);

            cinematiqueVideoPlayer.enabled = false;

            if (cinematiqueType == CinematiqueType.NEW_GAME) {
                GameManager.Instance.Start_New_Game();
            }
        }

        void EndReached(VideoPlayer videoPlayer) {
            Skip();
        }
    }
}
