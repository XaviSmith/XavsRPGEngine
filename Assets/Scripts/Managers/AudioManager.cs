using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioClip))]
public class AudioManager : Singleton<AudioManager> {

    public AudioSource myAudio;
    
    //Menu sounds
    public AudioClip cursorSound;
    public AudioClip menuSelectSound;

    //Dialogue sounds
    public AudioClip dialogueSound;

    //General Battle sounds
    public AudioClip attackSound;
    public AudioClip winFight;
    public AudioClip levelUp;

	// Use this for initialization
	void Start () {
        myAudio = GetComponent<AudioSource>();
	}
	
    public void PlaySound(AudioClip sound)
    {
        myAudio.PlayOneShot(sound);
    }
}
