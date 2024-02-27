using System;
using TMPro;
using UnityEngine;

public class PausedUI : MonoBehaviour {
	private void Start() {
		GameManager.instance.OnGamePaused += GameManager_OnGamePaused;
		GameManager.instance.OnGameUnpaused += GameManager_OnGameUnpaused;
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
