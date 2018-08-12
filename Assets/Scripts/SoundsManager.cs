using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
	AudioSource a;

	void Start ()
	{
		a = GetComponent<AudioSource>();
	}

	public void PlaySound(AudioClip clip)
	{
		a.PlayOneShot(clip);
	}
}
