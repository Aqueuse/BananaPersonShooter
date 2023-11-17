using Cinemachine;
using Enums;
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

        private CinematiqueType cinematiqueType;
        
        private void Start() {
            _cinematiqueVideoPlayer = GetComponentInChildren<VideoPlayer>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            _cinematiqueCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            
            RenderTexture.active = _cinematiqueVideoPlayer.targetTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = null;

            _cinematiqueVideoPlayer.loopPointReached += EndReached;
            _cinematiqueVideoPlayer.SetDirectAudioVolume(0, ObjectsReference.Instance.audioManager.musicLevel);
        }

        public void Play(CinematiqueType playedCinematiqueType) {
            cinematiqueType = playedCinematiqueType;
            ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.MUSIC);

            _cinematiqueVideoPlayer.enabled = true;
            skipCinematiqueGameObject.SetActive(true);

            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_CINEMATIQUE;

            _cinematiqueCamera.Priority = 200;
            _meshRenderer.material = playedCinematiqueType == CinematiqueType.DEATH ? transparentVideoMaterial : videoMaterial;

            _cinematiqueVideoPlayer.clip = cinematiquesVideoClipsByType[playedCinematiqueType];
            _cinematiqueVideoPlayer.frame = 0;
            _cinematiqueVideoPlayer.SetDirectAudioVolume(0, ObjectsReference.Instance.audioManager.musicLevel);
            _cinematiqueVideoPlayer.Play();
        }

        public void Pause() {
			if (_cinematiqueVideoPlayer.enabled)
				_cinematiqueVideoPlayer.Pause();
        }

        public void Unpause() {
            if(_cinematiqueVideoPlayer.enabled)
                _cinematiqueVideoPlayer.Play();
        }

        public void Skip() {
            _cinematiqueCamera.Priority = 3;

            skipCinematiqueGameObject.SetActive(false);

            _cinematiqueVideoPlayer.enabled = false;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1f;
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ObjectsReference.Instance.bananaMan.activeItem.bananaType);

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
            ObjectsReference.Instance.gameManager.isGamePlaying = true;

            ObjectsReference.Instance.cameraPlayer.Return_back_To_Player();
            ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();

            ObjectsReference.Instance.playerController.canMove = true;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = false;

            if (cinematiqueType == CinematiqueType.NEW_GAME) {
                ObjectsReference.Instance.teleportation.TeleportDown();
                ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.MUSIC);
                ObjectsReference.Instance.audioManager.PlayAmbiance(AmbianceType.DRONE_COMMANDROOM);
            }
            else {
                ObjectsReference.Instance.audioManager.SetMusiqueAndAmbianceBySceneName(ObjectsReference.Instance.mapsManager.currentMap.mapDataScriptableObject.sceneName);
            }
        }

        void EndReached(VideoPlayer videoPlayer) {
            Skip();
        }

        public void SetCinematiqueVolume() {
            _cinematiqueVideoPlayer.SetDirectAudioVolume(0, ObjectsReference.Instance.audioManager.musicLevel);
        }
    }
}
