using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
	public AudioClip[] audios;
	private AudioSource audioSource;

	void Awake ()
	{
		audioSource = GetComponent<AudioSource> ();
	}

	void Start ()
	{
		Random.InitState (System.DateTime.Now.Second);
		audioSource.PlayOneShot (audios[Random.Range (0, audios.Length - 1)]);
	}
}