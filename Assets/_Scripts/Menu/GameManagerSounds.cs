using System;
using UnityEngine;

public class GameManagerSounds : MonoBehaviour {
	private void OnEnable() {
		GameManager.instance.OnCountdownStarted += GameManager_OnCountdownStarted;
		GameManager.instance.OnCountdownChanged += GameManager_OnCountdownChanged;
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

}
