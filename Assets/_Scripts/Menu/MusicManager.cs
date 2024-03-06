using System;
using UnityEngine;

public class MusicManager : MonoBehaviour {
	private AudioSource m_audioSource;

	private void Awake() {
		m_audioSource = GetComponent<AudioSource>();
	}

	private void Start() {
		GameManager.instance.OnPlayingStarted += GameManager_OnPlayingStarted;
		GameManager.instance.OnGamePaused += GameManager_OnGamePaused;
		GameManager.instance.OnGameUnpaused += GameManager_OnGameUnpaused;

		m_audioSource.Stop();
	}

    private void GameManager_OnPlayingStarted(object sender, EventArgs e) {
		m_audioSource.Play();
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e) {
		m_audioSource.Pause();
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e) {
		m_audioSource.UnPause();
    }
}
