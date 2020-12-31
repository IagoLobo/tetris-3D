using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource musicSource;
    [SerializeField] private AudioClip musicClip;

    void Awake()
    {
        if (Instance != null)
		{
			Destroy(this.gameObject);
			return;
		}

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        musicSource = GetComponent<AudioSource>();
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }
}
