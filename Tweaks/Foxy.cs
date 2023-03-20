namespace UnityEngine
{
	public partial class Foxy : Debug
	{
		public static new void LogError(object message) => Debug.LogError($"<color=red>Error: </color>{message}, héhéhéhé");
		public static new void Log(object message) => Debug.Log($"<color=green>Good: </color>{message}, héhéhéhé");
		public static new void LogWarning(object message) => Debug.LogWarning($"<color=yellow>Good: </color>{message}, héhéhéhé");
	}
}

