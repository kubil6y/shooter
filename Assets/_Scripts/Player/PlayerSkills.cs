using System;
using UnityEngine;

public class PlayerSkills : MonoBehaviour {
	public event EventHandler OnUltimateOutOfCooldown;

	[Header("Ultimate Skill")]
	[SerializeField] private UltimateLaser m_ultimatePrefab;
	[SerializeField] private int m_ultimateLaserDamage;
	[SerializeField] private float m_ultimateCooldown;
	[SerializeField] private float m_playerSurroundingRadius = 5f;
	[SerializeField] private float m_ultimateRange = 25f;
	[SerializeField] private Transform m_ultimateSpawnTf;
	[SerializeField] private LayerMask m_ultimateLimitLayerMask;
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

	public void Dash(Vector3 dashDirection) {
        Vector3 dashPoint = m_player.transform.position + dashDirection * m_dashDistance;
        RaycastHit2D hit = Physics2D.Raycast(m_player.transform.position, dashDirection, m_dashDistance, m_dashLayerMask);
        if (hit.collider != null) {
            dashPoint = hit.point;
        }
        m_player.transform.position = dashPoint;
	}

	public void UltimatePlayerSurroundingAttack() {
		RaycastHit2D[] hits = Physics2D.CircleCastAll(m_player.transform.position, m_playerSurroundingRadius, Vector2.right, 0f, m_ultimateTargetLayerMask);
		foreach (RaycastHit2D hit in hits) {
			if (hit.collider != null) {
				hit.collider.GetComponent<IDamageable>()?.TakeDamage(m_ultimateLaserDamage * m_player.GetDamageMultiplier());
				float hitDuration = .1f;
				hit.collider.GetComponent<IHittable>()?.TakeHit(WeaponType.LightningGun, hitDuration);
			}
		}

	}

	public void SpawnLaser(Vector2 direction) {
		UltimateLaser ultimateLaser = Instantiate(m_ultimatePrefab);
		Vector2 ultimateSpawnPos = GetUltimateSpawnPosition();
		float ultimateRange = m_ultimateRange;
		float laserRange = ultimateRange;

		RaycastHit2D hit = Physics2D.Raycast(ultimateSpawnPos, direction, ultimateRange, m_ultimateLimitLayerMask);
		if (hit.collider != null) {
			laserRange = Vector2.Distance(hit.point, ultimateSpawnPos);
		}

		int damage = m_ultimateLaserDamage * m_player.GetDamageMultiplier();
		ultimateLaser.Setup(ultimateSpawnPos, direction, damage, laserRange);
	}


	public float GetUltimateTimerNormalized() {
		float val = 1 - m_ultimateTimer / m_ultimateCooldown;
		return val < 1f ? val : 0f;
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

	private void Player_OnAnimUltimateEnded(object sender, EventArgs e) {
		m_isUltimateOnCooldown = true;
		m_ultimateTimer = m_ultimateCooldown;
	}
}
