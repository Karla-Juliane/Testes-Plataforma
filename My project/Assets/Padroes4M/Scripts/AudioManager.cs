using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }

    public float volume = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float Volume
    {
        get
        {
            return volume;
        }
        set
        {
            volume = Mathf.Clamp01(value);
        }
    }
}
