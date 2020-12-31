using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private AudioSource sfxSource;
    [SerializeField] private AudioClip clearRowSFX;

    void Awake()
    {
        sfxSource = GetComponent<AudioSource>();
    }

    public void PlayClearRowSFX()
    {
        sfxSource.clip = clearRowSFX;
        sfxSource.Play();
    }
}
