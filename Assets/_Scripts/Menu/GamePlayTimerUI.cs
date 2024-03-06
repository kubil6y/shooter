using System;
using TMPro;
using UnityEngine;

public class GamePlayTimerUI : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI m_playTimerText;

	private void Start() {
		GameManager.instance.OnPlayingStarted += GameManager_OnPlayingStarted;
		GameManager.instance.OnGameOver += GameManager_OnGameOver;
		Hide();
	}

    private void Update() {
        UpdatePlayTimerText();
    }

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

    private void UpdatePlayTimerText() {
        if (GameManager.instance.IsPlaying()) {
            float t = GameManager.instance.GetPlayTimer();
            string minutes = ((int)t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");
            m_playTimerText.text = minutes + ":" + seconds;
        }
    }

    private void GameManager_OnPlayingStarted(object sender, EventArgs e) {
		Show();
    }

    private void GameManager_OnGameOver(object sender, EventArgs e) {
        Hide();
    }
}
