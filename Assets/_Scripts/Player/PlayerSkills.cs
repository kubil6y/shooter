using System;
using UnityEngine;

public class PlayerSkills : MonoBehaviour {
	public event EventHandler OnUltimateOutOfCooldown;

	[Header("Ultimate Skill")]
	[SerializeField] private UltimateLaser m_ultimatePrefab;
	[SerializeField] private int m_ultimateLaserDamage;
	[SerializeField] private float m_ultimateCooldown;
	[SerializeField] private float m_ultimateRange = 25f;
	[SerializeField] private Transform m_ultimateSpawnTf;
	[SerializeField] private LayerMask m_ultimateTargetLayerMask;

	private float m_ultimateTimer;

	[Header("Dash Skill")]
	[SerializeField] private float m_dashCooldown;
	[SerializeField] public float m_dashDistance;
	[SerializeField] private LayerMask m_dashLayerMask;
	private float m_dashTimer;

	private Player m_player;
	private bool m_isUltimateOnCooldown;

	private void Awake() {
		m_player = GetComponent<Player>();
	}

	private void Start() {
		m_player.animations.OnAnimUltimateEnded += Player_OnAnimUltimateEnded;
	}

	private void Update() {
		m_dashTimer -= Time.deltaTime;
		m_ultimateTimer -= Time.deltaTime;

		if (m_ultimateTimer < 0f && m_isUltimateOnCooldown) {
			m_isUltimateOnCooldown = false;
			OnUltimateOutOfCooldown?.Invoke(this, EventArgs.Empty);
		}

		if (m_player.IsAlive() && m_player.CanUseSkill()) {
			if (GameInput.instance.DashPressed() && m_dashTimer < 0f) {
				m_dashTimer = m_dashCooldown;
				m_player.SetState(PState.Dash);
			}

			if (GameInput.instance.UltimatePressed() && !m_isUltimateOnCooldown) {
				m_player.SetState(PState.Ultimate);
			}
		}
	}

	public float GetUltimateTimerNormalized() {
		return 1 - m_ultimateTimer / m_ultimateCooldown;
	}

	public LayerMask GetUltimateTargetLayerMask() {
		return m_ultimateTargetLayerMask;
	}

	public float GetDashDistance() {
		return m_dashDistance;
	}

	public LayerMask GetDashLayerMask() {
		return m_dashLayerMask;
	}

	public float GetUltimateRange() {
		return m_ultimateRange;
	}

	public Vector2 GetUltimateSpawnPosition() {
		if (m_player.IsFacingRight()) {
			return m_ultimateSpawnTf.position;
		}
		else {
			float x = m_ultimateSpawnTf.position.x - 2 * m_ultimateSpawnTf.localPosition.x;
			float y = m_ultimateSpawnTf.position.y;
			return new Vector2(x, y);
		}
	}

	public int GetUltimateLaserDamage() {
		return m_ultimateLaserDamage;
	}

	public UltimateLaser GetUltimateLaserPrefab() {
		return m_ultimatePrefab;
	}

	private void Player_OnAnimUltimateEnded(object sender, EventArgs e) {
		m_isUltimateOnCooldown = true;
		m_ultimateTimer = m_ultimateCooldown;
	}

}
