using System;
using UnityEngine;

public class GameManagerSounds : MonoBehaviour {
	private void OnEnable() {
		GameManager.instance.OnCountdownStarted += GameManager_OnCountdownStarted;
		GameManager.instance.OnCountdownChanged += GameManager_OnCountdownChanged;
		GameManager.instance.OnGameOver += GameManager_OnGameOver;
		GameManager.instance.OnGamePaused += GameManager_OnGamePaused;
		GameManager.instance.OnGameUnpaused += GameManager_OnGameUnpaused;
	}

    private void OnDisable() {
		GameManager.instance.OnCountdownStarted -= GameManager_OnCountdownStarted;
		GameManager.instance.OnCountdownChanged -= GameManager_OnCountdownChanged;
	}

	private void GameManager_OnCountdownStarted(object sender, EventArgs e) {
		AudioManager.instance.PlayAnnouncerStartLevel(transform.position);
	}

	private void GameManager_OnCountdownChanged(object sender, int countdown) {
		switch (countdown) {
		case 3:
			AudioManager.instance.PlayAnnouncerThree(transform.position);
			break;
		case 2:
			AudioManager.instance.PlayAnnouncerTwo(transform.position);
			break;
		case 1:
			AudioManager.instance.PlayAnnouncerOne(transform.position);
			break;
		case 0:
			AudioManager.instance.PlayAnnouncerFight(transform.position);
			break;
		default:
			break;
		}
	}

    private void GameManager_OnGameOver(object sender, EventArgs e) {
		AudioManager.instance.PlayGameOver(transform.position);
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e) {
		AudioManager.instance.PlayGamePaused(transform.position);
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e) {
		AudioManager.instance.PlayGameUnpaused(transform.position);
    }

}
