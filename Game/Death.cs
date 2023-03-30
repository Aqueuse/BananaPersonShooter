using Audio;
using Cameras;
using Enums;
using Input;
using Input.UIActions;
using Player;
using UI;
using UI.InGame;
using UnityEngine;
using UnityEngine.Video;

namespace Game {
    public class Death : MonoSingleton<Death> {
        [SerializeField] private VideoPlayer deathVideoPlayer;
        [SerializeField] private MeshRenderer deathPlaneMeshRenderer;
        
        public void Die() {
            if (GameManager.Instance.isGamePlaying) {
                GameManager.Instance.isGamePlaying = false;
                
                BananaMan.Instance.GetComponent<BananaMan>().Die();
                UIFace.Instance.Die(true);
                AudioManager.Instance.PlayEffect(EffectType.BANANASPLASH, 0);

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                InputManager.Instance.SwitchContext(InputContext.UI);
                MainCamera.Instance.Set0Sensibility();

                UIManager.Instance.Set_active(UICanvasGroupType.DEATH, true);

                deathPlaneMeshRenderer.enabled = true;
                deathVideoPlayer.enabled = true;

                UISchemaSwitcher.Instance.SwitchUISchema(UISchemaSwitchType.DEATH);
                GameManager.Instance.gameContext = GameContext.DEAD;
                
                deathVideoPlayer.frame = 0;
                deathVideoPlayer.Play();
            
                AudioManager.Instance.StopAudioSource(AudioSourcesType.MUSIC);
            }
        }

        public void HideDeath() {
            deathPlaneMeshRenderer.enabled = false;
        }
    }
}
