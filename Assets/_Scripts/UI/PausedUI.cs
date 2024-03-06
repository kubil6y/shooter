using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour {
	[SerializeField] private Button m_resumeButton;
	[SerializeField] private Button m_mainMenuButton;

	private void Start() {
		GameManager.instance.OnGamePaused += GameManager_OnGamePaused;
		GameManager.instance.OnGameUnpaused += GameManager_OnGameUnpaused;

		m_resumeButton.onClick.AddListener(() => {
			GameManager.instance.Unpause();
		});
		m_mainMenuButton.onClick.AddListener(() => {
			Loader.Load(Loader.Scene.MainMenuScene);
		});

		Hide();
	}

	private void Show() {
		gameObject.SetActive(true);
	}

	private void Hide() {
		gameObject.SetActive(false);
	}

	private void GameManager_OnGamePaused(object sender, EventArgs e) {
		Show();
	}

	private void GameManager_OnGameUnpaused(object sender, EventArgs e) {
		Hide();
	}
}
