using System;
using UnityEngine;

[CreateAssetMenu()]
public class SoundSO : ScriptableObject {
	public enum AudioType {
		SFX,
		Music,
	}

	public AudioType type;
	public AudioClip clip;
	public bool loop;
	public bool randomizedPitch;
	[Range(0f, 1f)]
	public float randomPitchRangeModifier = .1f;
	[Range(.1f, 2f)]
	public float volume = 1f;
	[Range(.1f, 3f)]
	public float pitch = 1f;
}
