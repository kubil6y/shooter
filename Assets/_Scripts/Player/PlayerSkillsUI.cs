using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillsUI : MonoBehaviour {
	[SerializeField] private Image m_ultimateImage;
	private bool m_isUltimateOnCooldown;

	private void Start() {
		Player.instance.animations.OnAnimUltimateFire += Player_OnAnimUltimateFire;
		Player.instance.skills.OnUltimateOutOfCooldown += Player_OnUltimateOutOfCooldown;
	}

	private void Update() {
        UpdateUltimateCooldownVisual();
    }

    private void UpdateUltimateCooldownVisual() {
        if (m_isUltimateOnCooldown) {
            m_ultimateImage.fillAmount = Player.instance.skills.GetUltimateTimerNormalized();
        }
    }

    private void Player_OnAnimUltimateFire(object sender, EventArgs e) {
		m_isUltimateOnCooldown = true;
	}

	private void Player_OnUltimateOutOfCooldown(object sender, EventArgs e) {
		m_isUltimateOnCooldown = false;
	}

}
