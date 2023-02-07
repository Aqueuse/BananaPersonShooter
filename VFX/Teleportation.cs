using UnityEngine;

namespace VFX {
    enum TeleportState {
        VISIBLE,
        INVISIBLE
    }
    
    public class Teleportation : MonoBehaviour {
        [SerializeField] private Material[] materialsToDissolve;

        [SerializeField] private float shaderSpeed;

        [SerializeField] private float dissolveFactor;

        [SerializeField] private RectTransform faceCanvasTransform;
        
        private static readonly int CutoffHeight = Shader.PropertyToID("Cutoff_Height");

        private ParticleSystem teleportParticleSystem;
        private TeleportState teleportState;
        ParticleSystem.ShapeModule shape;

        private void Start() {
            teleportState = TeleportState.VISIBLE;
        }

        private void Update() {
            if (teleportState == TeleportState.VISIBLE && dissolveFactor < 2) {
                ReappearVertical();
            }
            
            if (teleportState == TeleportState.INVISIBLE && dissolveFactor > -3.23f) {
                DisappearVertical();
            }
        }

        // dissolve factor : -3.23 = totally invisible
        // dissolve factor : 2 = totally visible

        private void DisappearVertical() {
            dissolveFactor -= Time.deltaTime * shaderSpeed;

            foreach (var material in materialsToDissolve) {
                material.SetFloat(CutoffHeight, dissolveFactor);
            }
        }
        
        private void ReappearVertical() {
            dissolveFactor += Time.deltaTime * shaderSpeed;

            foreach (var material in materialsToDissolve) {
                material.SetFloat(CutoffHeight, dissolveFactor);
            }
        }
        
        public void TeleportUp() {
            teleportParticleSystem = GetComponent<ParticleSystem>();
            shape = teleportParticleSystem.shape;

            teleportParticleSystem.Play();
            teleportState = TeleportState.INVISIBLE;
            dissolveFactor = 2;
            
            shape.rotation = new Vector3(0, 0, 0);
            
            Invoke(nameof(Hide_Face_Canvas), 1);
        }
        
        public void TeleportDown() {
            teleportParticleSystem = GetComponent<ParticleSystem>();
            shape = teleportParticleSystem.shape;

            teleportParticleSystem.Play();
            teleportState = TeleportState.VISIBLE;
            dissolveFactor = -3.23f;

            shape.rotation = new Vector3(0, 180, 0);
            
            Invoke(nameof(Show_Face_Canvas), 1);
        }

        private void Show_Face_Canvas() {
            faceCanvasTransform.localScale = new Vector3(0.05524086f, 0.05524086f, 0.05524086f);
        }
        
        private void Hide_Face_Canvas() {
            faceCanvasTransform.localScale = new Vector3(0, 0, 0);
        }
    }
}
