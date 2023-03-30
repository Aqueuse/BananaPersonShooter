using UnityEngine;

namespace VFX {
    public enum TeleportState {
        VISIBLE = 0,
        INVISIBLE = 1,
        DISAPPEAR = 2,
        REAPPEAR = 3
    }
    
    public class Teleportation : MonoSingleton<Teleportation> {
        [SerializeField] private Material[] materialsToDissolve;
        [SerializeField] private float shaderSpeed;
        [SerializeField] private RectTransform faceCanvasTransform;

        public TeleportState teleportState;
        
        private static readonly int CutoffHeight = Shader.PropertyToID("Cutoff_Height");
        private float _dissolveFactor;

        private ParticleSystem _teleportParticleSystem;
        private ParticleSystem.ShapeModule _shape;

        private void Start() {
            _dissolveFactor = 2f;
            teleportState = TeleportState.VISIBLE;
        }

        private void Update() {
            if (teleportState == TeleportState.REAPPEAR &&  _dissolveFactor < 2) {
                _dissolveFactor += Time.deltaTime * shaderSpeed;

                foreach (var material in materialsToDissolve) {
                    material.SetFloat(CutoffHeight, _dissolveFactor);
                }
            }

            if (teleportState == TeleportState.REAPPEAR &&  _dissolveFactor >= 2) {
                teleportState = TeleportState.VISIBLE;
                Show_Face_Canvas();
            }
            
            if (teleportState == TeleportState.DISAPPEAR && _dissolveFactor > -3.23f) {
                _dissolveFactor -= Time.deltaTime * shaderSpeed;

                foreach (var material in materialsToDissolve) {
                    material.SetFloat(CutoffHeight, _dissolveFactor);
                }
            }

            if (teleportState == TeleportState.DISAPPEAR && _dissolveFactor <= -3.23f) {
                teleportState = TeleportState.INVISIBLE;
            }
        }

        // dissolve factor : -3.23 = totally invisible
        // dissolve factor : 2 = totally visible
        
        public void TeleportUp() {
            _teleportParticleSystem = GetComponent<ParticleSystem>();
            _shape = _teleportParticleSystem.shape;

            _teleportParticleSystem.Play();
            teleportState = TeleportState.DISAPPEAR;
            _dissolveFactor = 2;

            foreach (var material in materialsToDissolve) {
                material.SetFloat(CutoffHeight, _dissolveFactor);
            }
            
            _shape.rotation = new Vector3(0, 0, 0);

            Hide_Face_Canvas();
        }

        public void TeleportDown() {
            _teleportParticleSystem = GetComponent<ParticleSystem>();
            _shape = _teleportParticleSystem.shape;

            _teleportParticleSystem.Play();
            teleportState = TeleportState.REAPPEAR;
            _dissolveFactor = -3.23f;
            
            foreach (var material in materialsToDissolve) {
                material.SetFloat(CutoffHeight, _dissolveFactor);
            }

            _shape.rotation = new Vector3(0, 180, 0);
        }

        public void ShowBananaManMaterials() {
            foreach (var material in materialsToDissolve) {
                material.SetFloat(CutoffHeight, 3.23f);
            }
        }

        private void Show_Face_Canvas() {
            faceCanvasTransform.localScale = new Vector3(0.05524086f, 0.05524086f, 0.05524086f);
        }
        
        private void Hide_Face_Canvas() {
            faceCanvasTransform.localScale = new Vector3(0, 0, 0);
        }
    }
}
