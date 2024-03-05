using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{

}

[DisallowMultipleComponent]
public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Singletone")]
    [SerializeField] private bool m_DoNotDestroyOnLoad;

    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            //Debug.LogError("На сцене LevelBoundary уже есть");
            Destroy(this);
            return;
        }

        Instance = this as T;
        
        if(m_DoNotDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }


}
