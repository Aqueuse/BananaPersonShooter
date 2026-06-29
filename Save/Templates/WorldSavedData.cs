using System.Collections.Generic;

namespace Save.Templates {
    public class WorldSavedData {
        public Dictionary<string, int> monkeysSasietyTimerByMonkeyId = new();
        public Dictionary<string, string> monkeysPositionByMonkeyId = new();

        public int stationLightSetting;
        public int lastVisitorGroup;

        public int bananaGoopQuantity;
        
        public RegionType activeCannonRegion;

        public Dictionary<string, float> cannonsSocleYRotation = new Dictionary<string, float>() {
            { RegionType.MAP01.ToString(), 0 },
            { RegionType.MAP02.ToString(), 0 },
            { RegionType.MAP03.ToString(), 0 },
            { RegionType.MAP04.ToString(), 0 },
            { RegionType.MAP05.ToString(), 0 },
            { RegionType.MAP06.ToString(), 0 },
            { RegionType.MAP07.ToString(), 0 },
            { RegionType.MAP08.ToString(), 0 }
        };
        
        public Dictionary<string, float> cannonsXRotation = new Dictionary<string, float>() {
            { RegionType.MAP01.ToString(), 0 },
            { RegionType.MAP02.ToString(), 0 },
            { RegionType.MAP03.ToString(), 0 },
            { RegionType.MAP04.ToString(), 0 },
            { RegionType.MAP05.ToString(), 0 },
            { RegionType.MAP06.ToString(), 0 },
            { RegionType.MAP07.ToString(), 0 },
            { RegionType.MAP08.ToString(), 0 }
        };
    }
}
