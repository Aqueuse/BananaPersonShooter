using System.Collections.Generic;
using UnityEngine;

namespace Tweaks {
    [RequireComponent(typeof(TrailRenderer))]
    public class TrailRendererRandom : MonoBehaviour {
        [SerializeField] private bool userColor, disableRandom;
        [SerializeField] private float widthMultiplier = 0.2f;
        [SerializeField, Range(1, 8)] private int keyNb = 8;
        [SerializeField] private List<Color> colorList = new();

        private float _alpha;
        private float _keyUpdate;
        private float _time;

        private readonly Gradient _gradient = new();
        private readonly AnimationCurve _curve = new();
        private TrailRenderer _trailRenderer;
        private GradientColorKey[] _colorKey;
        private GradientAlphaKey[] _alphaKey;

        private void Start() {
            _time = 0f;
            _alpha = 1.0f;
            _keyUpdate = 1.0f;

            userColor = false;
            disableRandom = false;
        
            _trailRenderer = gameObject.GetComponent<TrailRenderer>();
            _trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
            if (colorList.Count == 0) {
                userColor = false;
                _keyUpdate = 1f / keyNb;
                _trailRenderer.colorGradient = NewGrad(keyNb);

                for (var i = 0; i < _trailRenderer.colorGradient.colorKeys.Length; i++) {
                    colorList.Add(_trailRenderer.colorGradient.colorKeys[i].color);
                }
            }
            else {
                userColor = true;
                _keyUpdate = 1f / colorList.Count;
                _trailRenderer.colorGradient = NewGrad(colorList.Count);
            }

            NewCurve();

            _trailRenderer.emitting = false;
        }

        private void NewCurve() {
            _curve.AddKey(0.0f, 0.0f);
            _curve.AddKey(0.040f, 0.5f);
            _curve.AddKey(0.160f, 1.0f);
            _curve.AddKey(1.0f, 1.0f);
            _trailRenderer.widthCurve = _curve;
        }

        private void Update() {
            if (UnityEngine.Input.GetKeyUp(KeyCode.T) && !userColor) {
                _trailRenderer.emitting = !_trailRenderer.emitting;

                if (disableRandom) {
                    _alpha = 1.0f;
                    _time = 0.0f;
                    _keyUpdate = 1f;

                    colorList.Clear();
                    _keyUpdate = 1f / keyNb;
                    _trailRenderer.colorGradient = NewGrad(keyNb);

                    for (var i = 0; i < _trailRenderer.colorGradient.colorKeys.Length; i++) {
                        colorList.Add(_trailRenderer.colorGradient.colorKeys[i].color);
                    }
                }
            }
        }

        private void FixedUpdate() {
            _trailRenderer.widthMultiplier = widthMultiplier;
        }

        private Gradient NewGrad(int range) {
            _colorKey = new GradientColorKey[range];
            _alphaKey = new GradientAlphaKey[range];

            for (var i = 0; i < range; i++) {
                _colorKey[i].color = GetColor();
                _colorKey[i].time = _time;
                _alphaKey[i].alpha = _alpha;
                _alphaKey[i].time = _time;
                _time += _keyUpdate;
                _alpha -= _keyUpdate;
            }

            _gradient.SetKeys(_colorKey, _alphaKey);
            return _gradient;
        }

        private Color GetColor() {
            var r = (byte)Random.Range(0f, 255f);
            var g = (byte)Random.Range(0f, 255f);
            var b = (byte)Random.Range(0f, 255f);
            return new Color32(r, g, b, 1);
        }
    }
}