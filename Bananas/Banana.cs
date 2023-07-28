using System;
using Data.Bananas;
using Monkeys;
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
        
            if (collision.gameObject.CompareTag("Player")) {
                ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.BANANA_PEEL, 1);
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
