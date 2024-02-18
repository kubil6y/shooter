using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager> {
	[SerializeField] private AudioMixerGroup m_sfxMixerGroup;
	[SerializeField] private AudioMixerGroup m_musicMixerGroup;

	private float m_volume = 1f;
	private AudioSource m_currentMusic;

	private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

	private void PlaySound(AudioClip clip, float pitch, float volume, bool loop, AudioMixerGroup audioMixerGroup) {
		GameObject soundObject = new GameObject("Temp Audio Source");
		AudioSource audioSource = soundObject.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.pitch = pitch;
		audioSource.volume = volume;
		audioSource.loop = loop;
		audioSource.outputAudioMixerGroup = audioMixerGroup;
		audioSource.Play();

		if (!loop) {
			Destroy(soundObject, clip.length);
		}

		// Allow only one music at a time
		if (audioMixerGroup == m_musicMixerGroup) {
			if (m_currentMusic != null) {
				m_currentMusic.Stop();
			}
			m_currentMusic = audioSource;
		}
	}

	public AudioSource CreateAudioSource(AudioClip audioClip, Transform parentTf, bool loop, float volume, float volumeMultiplier = 1f) {
		GameObject soundObject = new GameObject("TempAudioSource");
		AudioSource audioSource = soundObject.AddComponent<AudioSource>();
		audioSource.clip = audioClip;
		audioSource.transform.parent = parentTf;
		audioSource.loop = loop;
		audioSource.volume = volume;
		audioSource.playOnAwake = false;
		return audioSource;
	}

	public void Play(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f) {
		AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * m_volume);
	}

	public void Play(AudioClip[] audioClips, Vector3 position, float volumeMultiplier = 1f) {
		int randomIndex = UnityEngine.Random.Range(0, audioClips.Length);
		AudioSource.PlayClipAtPoint(audioClips[randomIndex], position, volumeMultiplier * m_volume);
	}
}
