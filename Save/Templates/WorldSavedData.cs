using System.Collections.Generic;

namespace Save.Templates {
    public class WorldSavedData {
        public Dictionary<string, int> monkeysSasietyTimerByMonkeyId = new();
        public Dictionary<string, string> monkeysPositionByMonkeyId = new();

        public int stationLightSetting;
        public int lastVisitorGroup;
        
        public Dictionary<string, int> bananaCannonInventory = new Dictionary<string, int>() {
            { BananaType.BARANGAN.ToString(), 0 },
            { BananaType.BLUE_JAVA.ToString(), 0 },
            { BananaType.BURRO.ToString(), 0 },
            { BananaType.CAVENDISH.ToString(), 0 },
            { BananaType.GOLD_FINGER.ToString(), 0 },
            { BananaType.GROS_MICHEL.ToString(), 0 },
            { BananaType.LADY_FINGER.ToString(), 0 },
            { BananaType.MATOKE.ToString(), 0 },
            { BananaType.MUSA_VELUTINA.ToString(), 0 },
            { BananaType.NANJANGUD.ToString(), 0 },
            { BananaType.PISANG_RAJA.ToString(), 0 },
            { BananaType.PLANTAIN.ToString(), 0 },
            { BananaType.PRAYING_HANDS.ToString(), 0 },
            { BananaType.RED.ToString(), 0 },
            { BananaType.RINO_HORN.ToString(), 0 },
            { BananaType.TINDOK.ToString(), 0 }
        };
    }
}
