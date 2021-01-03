using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that controls the sound effects played during gameplay
public class SFXManager : MonoBehaviour
{
    private AudioSource sfxSource;
    [SerializeField] private AudioClip clearRowSFX;

    void Awake()
    {
        // Get audio source reference
        sfxSource = GetComponent<AudioSource>();
    }

    // Method that sets the ClearRow clip to the SFX audiosource and plays it
    public void PlayClearRowSFX()
    {
        sfxSource.clip = clearRowSFX;
        sfxSource.Play();
    }
}
