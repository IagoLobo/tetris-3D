using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that controls the game's music and instances
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource musicSource;
    [SerializeField] private AudioClip musicClip;

    void Awake()
    {
        // If there's another instance, destroy this one and stop code here
        if (Instance != null)
		{
			Destroy(this.gameObject);
			return;
		}

        // Else, this is the instance and don't destroy it between scenes
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Get audio source reference and set the music clip to play on loop
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }
}
