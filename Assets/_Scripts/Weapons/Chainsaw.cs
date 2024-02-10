using System;
using UnityEngine;

public class Chainsaw : Weapon {
	public event EventHandler OnShoot;
	public event EventHandler OnIdleStarted;
	public event EventHandler OnIdleEnded;

	[SerializeField] private float m_rof;
	[SerializeField] private Transform m_attackRefTf;
	[SerializeField] private float m_attackRadius = .75f;

	private bool m_isIdle;
	private float m_timer;

	private void Awake() {
		OnIdleStarted += Chainsaw_OnIdleStarted;
		OnIdleEnded += Chainsaw_OnIdleEnded;
	}

	protected virtual void Update() {
		if (!m_isIdle && m_timer > 0f && !shootInput) {
			m_isIdle = true;
			OnIdleStarted?.Invoke(this, EventArgs.Empty);
		}
		HandleAttack();
	}

	public override void SetAsCurrent() {
		base.SetAsCurrent();
		m_isIdle = true;
		OnIdleStarted?.Invoke(this, EventArgs.Empty);
	}

	private void HandleAttack() {
		m_timer -= Time.deltaTime;

		if (shootInput && m_timer < 0f) {
			m_timer = m_rof / 1000f;

			if (m_isIdle) {
				m_isIdle = false;
				OnIdleEnded?.Invoke(this, EventArgs.Empty);
			}

			Attack();
		}
	}

	private void Attack() {
		Debug.Log("Chainsaw:Raycast()");
		OnShoot?.Invoke(this, EventArgs.Empty);
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

	private void Chainsaw_OnIdleStarted(object sender, EventArgs e) {
		Debug.Log("Chainsaw:OnIdleStarted()");
	}

	private void Chainsaw_OnIdleEnded(object sender, EventArgs e) {
		Debug.Log("Chainsaw:OnIdleEnded()");
	}
}
