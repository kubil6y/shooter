using System;
using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI m_soulAmountText;
	[SerializeField] private TextMeshProUGUI m_killAmountText;

	private void Start() {
		Player.instance.OnSoulTaken += Player_OnSoulTaken;
		Player.instance.OnEnemyKillAmountChanged += Player_OnEnemyKillAmountChanged;
		SetSoulAmount(0);
		SetKillAmount(0);
	}

    private void SetKillAmount(int killAmount) {
		m_killAmountText.text = killAmount.ToString();
    }

    private void SetSoulAmount(int soulAmount) {
		m_soulAmountText.text = soulAmount.ToString();
	}

	private void Player_OnSoulTaken(object sender, int soulAmount) {
		SetSoulAmount(soulAmount);
	}

    private void Player_OnEnemyKillAmountChanged(object sender, int killAmount) {
		SetKillAmount(killAmount);
    }
}
