using System;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {
	[SerializeField] private float m_stepInterval = .3f;
	private Player m_player;
	private float m_stepTimer;

	private void Awake() {
		m_player = GetComponent<Player>();
	}

	private void Update() {
		m_stepTimer -= Time.deltaTime;

		if (m_player.IsMoving() && m_stepTimer < 0f) {
			m_stepTimer = m_stepInterval;
			AudioManager.instance.PlayPlayerStepSound(transform.position);
		}
	}

	private void Start() {
		m_player.OnDashStarted += Player_OnDashStarted;
		m_player.OnUltimated += Player_OnUltimated;
	}

    private void Player_OnDashStarted(object sender, EventArgs e) {
		AudioManager.instance.PlayPlayerDash(m_player.transform.position);
    }

    private void Player_OnUltimated(object sender, EventArgs e) {
		AudioManager.instance.PlayPlayerUltimate(m_player.transform.position);
    }
}
