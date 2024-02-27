using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillsUI : MonoBehaviour {
	[SerializeField] private Image m_backgroundImage;
	[SerializeField] private Image m_iconImage;
	[SerializeField] private Transform m_button;

	[SerializeField] private Color m_iconColor;
	[SerializeField] private Color m_onCooldownIconColor;
	[SerializeField] private Color m_buttonColor;
	[SerializeField] private Color m_onCooldownButtonColor;

	private bool m_isUltimateOnCooldown;

	private void Start() {
		Player.instance.animations.OnAnimUltimateFire += Player_OnAnimUltimateFire;
		Player.instance.skills.OnUltimateOutOfCooldown += Player_OnUltimateOutOfCooldown;
		SetBackgroundColor(onCooldown: false);
	}

	private void Update() {
		UpdateUltimateCooldownVisual();
	}

	private void UpdateUltimateCooldownVisual() {
		if (m_isUltimateOnCooldown) {
			m_backgroundImage.fillAmount = Player.instance.skills.GetUltimateTimerNormalized();
		}
	}

	private void ShowButton() {
		m_button.gameObject.SetActive(true);
	}

	private void HideButton() {
		m_button.gameObject.SetActive(false);
	}

	private void SetBackgroundColor(bool onCooldown) {
		if (onCooldown) {
			m_backgroundImage.color = m_onCooldownButtonColor;
			m_iconImage.color = m_onCooldownIconColor;
		}
		else {
			m_backgroundImage.color = m_buttonColor;
			m_iconImage.color = m_iconColor;
		}
	}

	private void Player_OnAnimUltimateFire(object sender, EventArgs e) {
		m_isUltimateOnCooldown = true;
		SetBackgroundColor(onCooldown: true);
		HideButton();
	}

	private void Player_OnUltimateOutOfCooldown(object sender, EventArgs e) {
		m_isUltimateOnCooldown = false;
		m_backgroundImage.fillAmount = 1f;
		SetBackgroundColor(onCooldown: false);
		ShowButton();
	}
}
