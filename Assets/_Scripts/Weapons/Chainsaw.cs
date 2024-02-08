using System;
using UnityEngine;

public class Chainsaw : Weapon {
	public event EventHandler OnShoot;
	public event EventHandler OnAmmoChanged;
	public event EventHandler OnIdleStarted;
	public event EventHandler OnIdleEnded;

	[SerializeField] private float m_rof;
	[SerializeField] private Transform m_attackRefTf;
	[SerializeField] private float m_attackRadius = .75f;

	private bool m_isIdle;
	private float m_timer;

	protected virtual void Update() {
		if (!m_isIdle && gameObject.activeSelf && !shootingInput && m_timer < 0f) {
			m_isIdle = true;
			OnIdleStarted?.Invoke(this, EventArgs.Empty);
		}
		HandleAttack();
	}

	private void HandleAttack() {
		m_timer -= Time.deltaTime;

		if (shootingInput && m_timer < 0f) {
			m_timer = m_rof / 1000f;

			Attack();

			if (m_isIdle && gameObject.activeSelf) {
				m_isIdle = false;
				OnIdleEnded?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	private void Attack() {
		Debug.Log("Chainsaw:Raycast()");
	}


	public override WeaponType GetWeaponType() {
		return WeaponType.Chainsaw;
	}

	public override bool IsOnCooldown() {
		return false;
	}

	private void OnDrawGizmos() {
		Gizmos.DrawWireSphere(m_attackRefTf.position, m_attackRadius);
	}
}
