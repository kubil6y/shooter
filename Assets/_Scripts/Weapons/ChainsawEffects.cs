using System;
using UnityEngine;

public class ChainsawEffects : MonoBehaviour {
	[SerializeField] private Chainsaw m_chainsaw;
	[SerializeField] private AudioSource m_idleAudioSource;
	[SerializeField] private AudioSource m_fireAudioSource;

	private void Update() {
		HandleAudio();
	}

	private void HandleAudio() {
		if (m_chainsaw.IsIdle()) {
			if (!m_idleAudioSource.isPlaying) {
				m_idleAudioSource.Play();
			}
			if (m_fireAudioSource.isPlaying) {
				m_fireAudioSource.Stop();
			}
		}
		else {
			if (m_idleAudioSource.isPlaying) {
				m_idleAudioSource.Stop();
			}
			if (!m_fireAudioSource.isPlaying) {
				m_fireAudioSource.Play();
			}
		}
	}
}
