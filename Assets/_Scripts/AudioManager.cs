using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager> {
	[SerializeField] private AudioClipRefsSO m_audioClipRefsSO;

	[SerializeField] private AudioMixerGroup m_sfxMixerGroup;
	[SerializeField] private AudioMixerGroup m_musicMixerGroup;

	[SerializeField] private SoundCollection m_soundCollection;

	private float m_masterVolume = .3f;
	private AudioSource m_currentMusic;

	private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

	public AudioSource CreateAudioSource(SoundSO soundSO, Transform parentTf = null) {
		GameObject soundObject = new GameObject("TempAudioSource");
		AudioSource audioSource = soundObject.AddComponent<AudioSource>();
		if (parentTf) {
			audioSource.transform.parent = parentTf;
		}
		audioSource.clip = soundSO.clip;
		audioSource.loop = soundSO.loop;
		audioSource.playOnAwake = soundSO.playOnAwake;
		audioSource.volume = m_masterVolume * soundSO.volume;

		float pitch = soundSO.pitch;
		if (soundSO.randomizedPitch) {
			float randomPitchModifier = UnityEngine.Random.Range(-soundSO.randomPitchRangeModifier, soundSO.randomPitchRangeModifier);
			pitch = soundSO.pitch + randomPitchModifier;
		}
		audioSource.pitch = pitch;

		AudioMixerGroup audioMixerGroup;
		switch (soundSO.type) {
		case SoundSO.AudioType.Music:
			audioMixerGroup = m_musicMixerGroup;
			break;
		default:
		case SoundSO.AudioType.SFX:
			audioMixerGroup = m_sfxMixerGroup;
			break;
		};
		audioSource.outputAudioMixerGroup = audioMixerGroup;

		return audioSource;
	}

	public void PlaySound(SoundSO soundSO, Vector3 position, Transform parentTf = null) {
		AudioSource audioSource = CreateAudioSource(soundSO, parentTf);
		audioSource.transform.position = position;
		audioSource.Play();

		if (!soundSO.loop) {
			Destroy(audioSource.gameObject, soundSO.clip.length);
		}

		// Allow only one music at a time
		if (soundSO.type == SoundSO.AudioType.Music) {
			m_currentMusic?.Stop();
			m_currentMusic = audioSource;
		}
	}


	public AudioSource CreateLGIdleLoopAudioSource(Transform parentTf) {
		AudioSource audioSource = CreateAudioSource(m_soundCollection.lgHum, parentTf);
		return audioSource;
	}

	public AudioSource CreateLGFireLoopAudioSource(Transform parentTf) {
		AudioSource audioSource = CreateAudioSource(m_soundCollection.lgFireLoop, parentTf);
		return audioSource;
	}

	public void PlayLGShootStarted(Vector3 position) {
		PlaySound(m_soundCollection.lgFireStart, position);
	}
}
