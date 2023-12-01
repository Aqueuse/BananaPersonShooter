using UnityEngine;
using UnityEngine.Video;

namespace Game {
    public class Death : MonoBehaviour {
        [SerializeField] private VideoPlayer deathVideoPlayer;
        [SerializeField] private MeshRenderer deathPlaneMeshRenderer;
        
        public void Die() {
            if (ObjectsReference.Instance.gameManager.isGamePlaying) {
                ObjectsReference.Instance.gameManager.isGamePlaying = false;
                
                ObjectsReference.Instance.uiFace.Die(true);
                ObjectsReference.Instance.bananaMan.tpsPlayerAnimator.FallFrontward();
                ObjectsReference.Instance.playerController.StopPlayer();
                
                ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
                ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.BANANASPLASH, 0);

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                ObjectsReference.Instance.cameraPlayer.Set0Sensibility();

                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.DEATH, true);

                deathPlaneMeshRenderer.enabled = true;
                deathVideoPlayer.enabled = true;

                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

                ObjectsReference.Instance.gameManager.gameContext = GameContext.DEAD;
                
                deathVideoPlayer.frame = 0;
                deathVideoPlayer.Play();
            }
        }

        public void HideDeath() {
            deathPlaneMeshRenderer.enabled = false;
            ObjectsReference.Instance.bananaMan.tpsPlayerAnimator.GetUp();
            ObjectsReference.Instance.uiFace.Die(false);
        }
    }
}
