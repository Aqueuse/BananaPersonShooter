using System;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    [Serializable]
    public class BananaDryerSlot {
        public bool hasBananaPeel;
        public bool hasFabric;
        public float timer;
        public BananaEffect bananaEffect;
    }
}
