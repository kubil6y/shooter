using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI m_enemyKilledCountText;
	[SerializeField] private TextMeshProUGUI m_soulsCountText;
	[SerializeField] private Button m_restartButton;
	[SerializeField] private Button m_quitButton;

	private void Start() {
		GameManager.instance.OnGameOver += GameManager_OnGameOver;

		m_quitButton.onClick.AddListener(() => {
			Application.Quit();
		});

		m_restartButton.onClick.AddListener(() => {
			// TODO fix this
			// SceneManager.LoadScene("GameScene");
			Loader.Load(Loader.Scene.GameScene);
		});

		Hide();
	}

	private void Show() {
		gameObject.SetActive(true);
	}

	private void Hide() {
		gameObject.SetActive(false);
	}

	private void GameManager_OnGameOver(object sender, EventArgs e) {
		m_soulsCountText.text = Player.instance.GetSoulAmount().ToString();
		m_enemyKilledCountText.text = Player.instance.GetKillAmount().ToString();
		Show();
	}
}
