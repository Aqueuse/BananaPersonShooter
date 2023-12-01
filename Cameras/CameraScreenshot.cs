using System.IO;
using UnityEngine;

namespace Cameras {
    public class CameraScreenshot : MonoBehaviour {
        [SerializeField] private Camera screenshotCamera;
        [SerializeField] private int width;
        [SerializeField] private int height;

        private string capturePath;
        
        private void Start() {
            capturePath = Path.Combine(Application.dataPath, "Sprites");
            capturePath = Path.Combine(capturePath, "Captures");
            capturePath = Path.Combine(capturePath, "camera_screenshot.png");
            capturePath = Path.GetFullPath(capturePath);

            SaveCameraView();
        }

        private void SaveCameraView() {
            var screenTexture = new RenderTexture(width, height, 16);
            screenshotCamera.targetTexture = screenTexture;
            RenderTexture.active = screenTexture;
            screenshotCamera.Render();
            var renderedTexture = new Texture2D(width, height);
            renderedTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            RenderTexture.active = null;
            var byteArray = renderedTexture.EncodeToPNG();
            File.WriteAllBytes(capturePath, byteArray);
            screenshotCamera.targetTexture = null;
        }
    }
}
