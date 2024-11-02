using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T _instance;

    public static T Instance => _instance;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        }
        else {
            _instance = this as T;
        }

        Init();
    }

    public virtual void Init() { }

    private void OnApplicationQuit() {
        _instance = null;
    }
}