using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioSource musicSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();

        }
    }


    public void ChangeBGM(AudioClip BGM) {
        musicSource.clip = BGM;
        //add a fade?
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

}
