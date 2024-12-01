using System.Collections.Generic;
using UnityEngine;

namespace InGame.Player {
    public class BananaManData : MonoBehaviour {
        public DroppedType activeDropped = DroppedType.EMPTY;
        
        public BananaType activeBanana = BananaType.EMPTY;
        public RawMaterialType activeRawMaterial = RawMaterialType.EMPTY;
        public IngredientsType activeIngredient = IngredientsType.EMPTY;
        public ManufacturedItemsType activeManufacturedItem = ManufacturedItemsType.EMPTY;
        public BuildableType activeBuildable = BuildableType.EMPTY;
        
        public int bitKongQuantity;
    }
}
