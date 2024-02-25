using System;
using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI m_healthText;
	[SerializeField] private TextMeshProUGUI m_armorText;

	private void Start() {
		Player.instance.health.OnHealthChanged += Player_OnHealthChanged;
		Player.instance.health.OnArmorChanged += Player_OnArmorChanged;
		UpdateHealthAndArmor();
	}

    private void UpdateHealthAndArmor() {
		m_healthText.text = Player.instance.health.GetCurrentHealth().ToString();
		m_armorText.text = Player.instance.health.GetCurrentArmor().ToString();
	}

    private void Player_OnHealthChanged(object sender, EventArgs e) {
		UpdateHealthAndArmor();
    }

    private void Player_OnArmorChanged(object sender, EventArgs e) {
		UpdateHealthAndArmor();
    }

}
