using System;
using UI.InGame.Inventory;
using UnityEngine;

namespace UI.InGame {
    public enum MouseAreaContext {
        INVENTORIES,
        GAMEZONE,
        QUICKSLOTS
    }
    
    public class UIHud : MonoBehaviour {
        public Transform buildablePlacementTransform;

        public bool isDragingSlot;
        public UInventorySlot draggedSlot;

        public MouseAreaContext mouseAreaContext;
        
        private Vector3 pivotLocalePosition;
        private Quaternion pivotLocaleRotation;
        private Vector3 raycastHitPointLocalePosition;
        private float deltaZ;
        private float deltaY;

        private Vector3 offsettedPosition;
        private Vector3 raycastHitPoint;
        
        public void SetMouseAreaContext(string mouseAreaContextString) {
            mouseAreaContext = (MouseAreaContext)Enum.Parse(typeof(MouseAreaContext), mouseAreaContextString);
        }
    }
}
