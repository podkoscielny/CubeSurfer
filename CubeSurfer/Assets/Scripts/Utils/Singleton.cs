using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }

    protected bool isDestroyableOnLoad = true;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this as T;
            if(!isDestroyableOnLoad) DontDestroyOnLoad(gameObject);
        }
    }
}
