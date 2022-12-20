using UnityEngine;

/// <summary>
/// Helper to create singleton, public class UneClasse : MonoSingleton<UneClasse>
/// </summary>
/// <typeparam name="T"></typeparam>
///
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance {
        get {
            // Instance requiered for the first time, we look for it
            if (_instance == null) {
                Debug.Log(typeof(T) + " is NULL");
                _instance = FindObjectOfType(typeof(T)) as T;
            }

            return _instance;
        }
    }

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