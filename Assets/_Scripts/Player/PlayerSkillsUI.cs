using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillsUI : MonoBehaviour {
	[SerializeField] private Image m_backgroundImage;
	[SerializeField] private Image m_ultimateImage;
	[SerializeField] private Transform m_button;
	private bool m_isUltimateOnCooldown;

	private void Start() {
		Player.instance.animations.OnAnimUltimateFire += Player_OnAnimUltimateFire;
		Player.instance.skills.OnUltimateOutOfCooldown += Player_OnUltimateOutOfCooldown;
		ShowBackground();
	}

	private void Update() {
		UpdateUltimateCooldownVisual();
	}

	private void UpdateUltimateCooldownVisual() {
		if (m_isUltimateOnCooldown) {
			m_ultimateImage.fillAmount = Player.instance.skills.GetUltimateTimerNormalized();
		}
	}

	private void ShowButton() {
		m_button.gameObject.SetActive(true);
	}

	private void HideButton() {
		m_button.gameObject.SetActive(false);
	}

	private void ShowBackground() {
		m_backgroundImage.gameObject.SetActive(true);
	}

	private void HideBackground() {
		m_backgroundImage.gameObject.SetActive(false);
	}

	private void Player_OnAnimUltimateFire(object sender, EventArgs e) {
		m_isUltimateOnCooldown = true;
		HideBackground();
		HideButton();
	}

	private void Player_OnUltimateOutOfCooldown(object sender, EventArgs e) {
		m_isUltimateOnCooldown = false;
		m_ultimateImage.fillAmount = 1f;
		ShowBackground();
		ShowButton();
	}
}
