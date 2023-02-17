using UnityEngine;

namespace VFX {
    public enum TeleportState {
        VISIBLE,
        INVISIBLE,
        DISAPPEAR,
        REAPPEAR
    }
    
    public class Teleportation : MonoSingleton<Teleportation> {
        [SerializeField] private Material[] materialsToDissolve;
        [SerializeField] private float shaderSpeed;
        [SerializeField] private RectTransform faceCanvasTransform;

        public TeleportState teleportState;
        
        private static readonly int CutoffHeight = Shader.PropertyToID("Cutoff_Height");
        private float dissolveFactor;

        private ParticleSystem teleportParticleSystem;
        private ParticleSystem.ShapeModule shape;

        private void Start() {
            dissolveFactor = 2f;
            teleportState = TeleportState.VISIBLE;
        }

        private void Update() {
            if (teleportState == TeleportState.REAPPEAR &&  dissolveFactor < 2) {
                dissolveFactor += Time.deltaTime * shaderSpeed;

                foreach (var material in materialsToDissolve) {
                    material.SetFloat(CutoffHeight, dissolveFactor);
                }
            }

            if (teleportState == TeleportState.REAPPEAR &&  dissolveFactor >= 2) {
                teleportState = TeleportState.VISIBLE;
                Show_Face_Canvas();
            }
            
            if (teleportState == TeleportState.DISAPPEAR && dissolveFactor > -3.23f) {
                dissolveFactor -= Time.deltaTime * shaderSpeed;

                foreach (var material in materialsToDissolve) {
                    material.SetFloat(CutoffHeight, dissolveFactor);
                }
            }

            if (teleportState == TeleportState.DISAPPEAR && dissolveFactor <= -3.23f) {
                teleportState = TeleportState.INVISIBLE;
            }
        }

        // dissolve factor : -3.23 = totally invisible
        // dissolve factor : 2 = totally visible
        
        public void TeleportUp() {
            teleportParticleSystem = GetComponent<ParticleSystem>();
            shape = teleportParticleSystem.shape;

            teleportParticleSystem.Play();
            teleportState = TeleportState.DISAPPEAR;
            dissolveFactor = 2;

            foreach (var material in materialsToDissolve) {
                material.SetFloat(CutoffHeight, dissolveFactor);
            }
            
            shape.rotation = new Vector3(0, 0, 0);

            Hide_Face_Canvas();
        }

        public void TeleportDown() {
            teleportParticleSystem = GetComponent<ParticleSystem>();
            shape = teleportParticleSystem.shape;

            teleportParticleSystem.Play();
            teleportState = TeleportState.REAPPEAR;
            dissolveFactor = -3.23f;
            
            foreach (var material in materialsToDissolve) {
                material.SetFloat(CutoffHeight, dissolveFactor);
            }

            shape.rotation = new Vector3(0, 180, 0);
        }

        private void Show_Face_Canvas() {
            faceCanvasTransform.localScale = new Vector3(0.05524086f, 0.05524086f, 0.05524086f);
        }
        
        private void Hide_Face_Canvas() {
            faceCanvasTransform.localScale = new Vector3(0, 0, 0);
        }
    }
}
