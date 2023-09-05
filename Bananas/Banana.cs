﻿using Data.Bananas;
using Enums;
using Tags;
using UnityEngine;

namespace Bananas {
    public class Banana : MonoBehaviour {
        [SerializeField] private GameObject bananaSkin;
        [SerializeField] private MeshRenderer bananaMeshRenderer;

        [SerializeField] private LayerMask bananaSplashLayerMask;
        
        public BananasDataScriptableObject bananasDataScriptableObject;
        
        private void OnCollisionEnter(Collision collision) {
            if (bananaSplashLayerMask == (bananaSplashLayerMask | 1 << collision.gameObject.layer)) {
                // trasnformation en peau de banane
                bananaMeshRenderer.enabled = false;
                bananaSkin.SetActive(true);

                Invoke(nameof(DestroyMe), 10);
                return;
            }

            if (TagsManager.Instance.HasTag(collision.gameObject, GAME_OBJECT_TAG.PLAYER)) {
                ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(RawMaterialType.BANANA_PEEL, 1);
                DestroyMe();
                return;
            }

            Invoke(nameof(DestroyMe), 10);
        }

        private void DestroyMe() {
            Destroy(transform.gameObject);
        }
    }
}
