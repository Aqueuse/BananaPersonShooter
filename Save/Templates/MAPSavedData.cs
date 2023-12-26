using System.Collections.Generic;

namespace Save.Templates {
    public class MapSavedData {
        public bool isDiscovered = false;
        
        public Dictionary<string, int> monkeysSasietyTimerByMonkeyId = new();
        public Dictionary<string, string> monkeysPositionByMonkeyId = new();
        
        public int piratesDebris = 0;
        public int visitorsDebris = 0;

        public int piratesQuantity;
        public int visitorsQuantity;
        public int chimployeesQuantity;
    }
}
