﻿using UnityEngine;

namespace InGame.Interactions {
    public class RotateTransform : MonoBehaviour {
        private Transform _elementToRotateTransform;
        private const float _speed = 200;

        private Vector3 rotationEuler = Vector3.forward;
        private Quaternion rotation;
        
        private void Start() {
            _elementToRotateTransform = transform;
            Invoke(nameof(DisableMe), 1.5f);
        }

        private void Update() {
            rotationEuler.z -= Time.deltaTime * _speed;
            if (rotationEuler.z < -360) rotationEuler.z = 0;
            
            rotation = Quaternion.Euler(rotationEuler);

            _elementToRotateTransform.localRotation = rotation;
        }

        private void DisableMe() {
            enabled = false;
        }
    }
}
