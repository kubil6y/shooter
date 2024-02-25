using System;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {
	[SerializeField] private float m_stepInterval = .35f;
	private Player m_player;
	private float m_stepTimer;

	private void Awake() {
		m_player = GetComponent<Player>();
	}

	private void Update() {
		m_stepTimer -= Time.deltaTime;

		if (m_player.IsMoving() && m_stepTimer < 0f) {
			m_stepTimer = m_stepInterval;
			if (m_player.HasQuad()) {
				m_stepTimer *= .9f;
			}
			AudioManager.instance.PlayPlayerStepSound(transform.position);
		}
	}

	private void Start() {
		m_player.OnDashStarted += Player_OnDashStarted;
		m_player.OnUltimated += Player_OnUltimated;
		m_player.OnSoulTaken += Player_OnSoulTaken;
		m_player.health.OnPickupHealth += Player_OnPickupHealth;
		m_player.health.OnPickupArmor += Player_OnPickupArmor;
		m_player.skills.OnUltimateOutOfCooldown += Player_OnUltimateOutOfCooldown;
	}

    private void Player_OnDashStarted(object sender, EventArgs e) {
		AudioManager.instance.PlayPlayerDash(m_player.transform.position);
	}

	private void Player_OnUltimated(object sender, EventArgs e) {
		AudioManager.instance.PlayPlayerUltimate(m_player.transform.position);
	}

	private void Player_OnSoulTaken(object sender, EventArgs e) {
		AudioManager.instance.PlayTakeSoul(transform.position);
	}

    private void Player_OnPickupHealth(object sender, EventArgs e) {
		AudioManager.instance.PlayHealthPickup(transform.position);
    }

    private void Player_OnPickupArmor(object sender, EventArgs e) {
		AudioManager.instance.PlayArmorPickup(transform.position);
    }

    private void Player_OnUltimateOutOfCooldown(object sender, EventArgs e) {
		AudioManager.instance.PlayUltimateOutOfCooldown(transform.position);
    }

}
