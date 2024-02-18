using System;
using UnityEngine;

public class Chainsaw : Weapon {
	public event EventHandler OnShootStarted;
	public event EventHandler OnShootEnded;
	public event EventHandler OnIdleStarted;
	public event EventHandler OnIdleEnded;
	public event EventHandler<Vector2> OnCut;

	[SerializeField] private int m_damagePerTick = 25;
	[SerializeField] private float m_rof = 200f;
	[SerializeField] private float m_attackRadius = .5f;
	[SerializeField] private Transform m_attackRefTf;
	[SerializeField] private LayerMask m_targetLayerMask;

	private bool m_isIdle;
	private bool m_isShooting;

	private float m_idleWaitDuration = .5f;
	private float m_shootTimer;
	private float m_idleTimer;

	private void Update() {
		m_shootTimer -= Time.deltaTime;
		m_idleTimer -= Time.deltaTime;

		if (!m_isShooting && shootInput) {
			if (m_isIdle) {
				m_isIdle = false;
				OnIdleEnded?.Invoke(this, EventArgs.Empty);
			}
			m_isShooting = true;
			OnShootStarted?.Invoke(this, EventArgs.Empty);
		}
		else if (m_isShooting && !shootInput) {
			m_isShooting = false;
			OnShootEnded?.Invoke(this, EventArgs.Empty);
		}

		if (m_isShooting && m_shootTimer < 0f) {
			m_shootTimer = m_rof / 1000f;
			m_idleTimer = m_idleWaitDuration;
			Shoot();
		}

		if (!m_isShooting && !m_isIdle && m_idleTimer < 0f) {
			m_isIdle = true;
			OnIdleStarted?.Invoke(this, EventArgs.Empty);
		}
	}

	private void Shoot() {
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_attackRefTf.position, m_attackRadius, m_targetLayerMask);

		foreach (Collider2D collider in colliders) {
			if (collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy)) {
				float hitDuration = .05f;
				collider.GetComponent<IHittable>()?.TakeHit(hitDuration);

				int damage  = m_damagePerTick * weaponUser.GetDamageMultiplier();
				collider.GetComponent<IDamageable>()?.TakeDamage(damage);

				OnCut?.Invoke(this, m_attackRefTf.transform.position);
			}
		}
	}

	public override void SetAsCurrent() {
		base.SetAsCurrent();
		m_isIdle = true;
		m_idleTimer = 0f;
		OnIdleStarted?.Invoke(this, EventArgs.Empty);
	}

	public override WeaponType GetWeaponType() {
		return WeaponType.Chainsaw;
	}

	public override bool IsOnCooldown() {
		return false;
	}

	public override bool IsAvailable() {
		return true;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(m_attackRefTf.position, m_attackRadius);
	}
}
