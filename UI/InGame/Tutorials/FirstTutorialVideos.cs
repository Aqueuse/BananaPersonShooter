using UnityEngine;
using UnityEngine.Video;

namespace UI.InGame.Tutorials {
    public class FirstTutorialVideos : MonoBehaviour {
        [SerializeField] private VideoPlayer grabVideoPlayer;
        [SerializeField] private VideoPlayer putVideoPlayer;

        private CanvasGroup tutorialCanvasGroup;

        private void Start() {
            tutorialCanvasGroup = GetComponent<CanvasGroup>();
        }

        public void Show() {
            UIManager.Instance.Set_active(tutorialCanvasGroup, true);

            grabVideoPlayer.loopPointReached += EndReached;
            putVideoPlayer.loopPointReached += EndReached;
        
            grabVideoPlayer.Play();
        }

        public void Hide() {
            UIManager.Instance.Set_active(tutorialCanvasGroup, false);
        
            grabVideoPlayer.loopPointReached -= EndReached;
            putVideoPlayer.loopPointReached -= EndReached;
        }

        void EndReached(VideoPlayer videoPlayer) {
            if (videoPlayer == grabVideoPlayer) {
                grabVideoPlayer.Play();
                grabVideoPlayer.Stop();
                
                putVideoPlayer.Play();
            }

            else {
                putVideoPlayer.Play();
                putVideoPlayer.Stop();
                
                grabVideoPlayer.Play();
            }
        }
    }
}
