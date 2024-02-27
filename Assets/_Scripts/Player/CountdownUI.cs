using System;
using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI m_countdownText;

	private void Start() {
		GameManager.instance.OnCountdownStarted += GameManager_OnCountdownStarted;
		GameManager.instance.OnCountdownChanged += GameManager_OnCountdownChanged;
		GameManager.instance.OnPlayingStarted += GameManager_OnPlayingStarted;
		Hide();
	}

	private void Show() {
		gameObject.SetActive(true);
	}

	private void Hide() {
		gameObject.SetActive(false);
	}

	private void SetCountdownText(int countdown) {
		m_countdownText.text = countdown.ToString();
	}

	private void GameManager_OnCountdownStarted(object sender, EventArgs e) {
		Show();
	}

	private void GameManager_OnCountdownChanged(object sender, int countdown) {
		SetCountdownText(countdown);
	}

	private void GameManager_OnPlayingStarted(object sender, EventArgs e) {
		Hide();
	}
}
