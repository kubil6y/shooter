using System;
using UnityEngine;

public class Chainsaw : Weapon {
	public event EventHandler OnShootStarted;
	public event EventHandler OnShootEnded;
	public event EventHandler OnIdleStarted;
	public event EventHandler OnIdleEnded;

	[SerializeField] private float m_rof = 200f;

	private bool m_isIdle;
	private bool m_isShooting;

	private float m_idleWaitDuration = .5f;
	private float m_shootTimer;
	private float m_idleTimer;


	private void Awake() {
		OnShootStarted += Chainsaw_OnShootStarted;
		OnShootEnded += Chainsaw_OnShootEnded;
		OnIdleStarted += Chainsaw_OnIdleStarted;
		OnIdleEnded += Chainsaw_OnIdleEnded;
	}

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
		Debug.Log("Shoot!");
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

	private void Chainsaw_OnShootStarted(object sender, EventArgs e) {
		Debug.Log("Chainsaw_OnShootStarted()");
	}

	private void Chainsaw_OnShootEnded(object sender, EventArgs e) {
		Debug.Log("Chainsaw_OnShootEnded()");
	}

	private void Chainsaw_OnIdleStarted(object sender, EventArgs e) {
		Debug.Log("Chainsaw_OnIdleStarted()");
	}

	private void Chainsaw_OnIdleEnded(object sender, EventArgs e) {
		Debug.Log("Chainsaw_OnIdleEnded()");
	}

    public override bool CanBeUsed() {
		return true;
    }
}
