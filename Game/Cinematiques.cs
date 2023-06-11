using Cinemachine;
using Enums;
using Input.UIActions;
using UnityEngine;
using UnityEngine.Video;

namespace Game {
    public class Cinematiques : MonoBehaviour {
        [SerializeField] private GenericDictionary<CinematiqueType, VideoClip> cinematiquesVideoClipsByType;

        [SerializeField] private Material videoMaterial;
        [SerializeField] private Material transparentVideoMaterial;

        [SerializeField] private GameObject skipCinematiqueGameObject;
        
        private VideoPlayer _cinematiqueVideoPlayer;
        private MeshRenderer _meshRenderer;
        private CinemachineVirtualCamera _cinematiqueCamera;

        private CinematiqueType _cinematiqueType;
        
        private void Start() {
            _cinematiqueVideoPlayer = GetComponentInChildren<VideoPlayer>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            _cinematiqueCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            
            RenderTexture.active = _cinematiqueVideoPlayer.targetTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = null;

            _cinematiqueVideoPlayer.loopPointReached += EndReached;
        }

        public void Play(CinematiqueType playedCinematiqueType) {
            _cinematiqueVideoPlayer.enabled = true;
            skipCinematiqueGameObject.SetActive(true);

            UISchemaSwitcher.Instance.SwitchUISchema(UISchemaSwitchType.CINEMATIQUE);
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_CINEMATIQUE;

            _cinematiqueType = playedCinematiqueType;
            _cinematiqueCamera.Priority = 200;
            _meshRenderer.material = playedCinematiqueType == CinematiqueType.DEATH ? transparentVideoMaterial : videoMaterial;

            _cinematiqueVideoPlayer.clip = cinematiquesVideoClipsByType[playedCinematiqueType];
            _cinematiqueVideoPlayer.frame = 0;
            _cinematiqueVideoPlayer.Play();
            
            ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.MUSIC);
        }

        public void Pause() {
			if (_cinematiqueVideoPlayer.enabled)
				_cinematiqueVideoPlayer.Pause();
        }

        public void Unpause() {
            if(_cinematiqueVideoPlayer.enabled)
                _cinematiqueVideoPlayer.Play();
            UISchemaSwitcher.Instance.SwitchUISchema(UISchemaSwitchType.CINEMATIQUE);
        }

        public void Skip() {
            _cinematiqueCamera.Priority = 3;
            
            skipCinematiqueGameObject.SetActive(false);

            _cinematiqueVideoPlayer.enabled = false;

            if (_cinematiqueType == CinematiqueType.NEW_GAME) {
                ObjectsReference.Instance.gameManager.Play(null, true);
            }
        }

        void EndReached(VideoPlayer videoPlayer) {
            Skip();
        }
    }
}
