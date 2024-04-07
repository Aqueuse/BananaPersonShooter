using System.Collections.Generic;

namespace Save.Templates {
    public class WorldSavedData {
        public Dictionary<string, int> monkeysSasietyTimerByMonkeyId = new();
        public Dictionary<string, string> monkeysPositionByMonkeyId = new();
        
        public int piratesQuantity;
        public int visitorsQuantity;
        public int chimployeesQuantity;
    }
}
