using System;
using UnityEngine;

public class LightningGun : Weapon, IHasAmmo {
	public event EventHandler OnShoot;
	public event EventHandler OnAmmoChanged;
	public event EventHandler OnIdleStarted;
	public event EventHandler OnIdleEnded;

	[SerializeField] private LightningGunWeaponDataSO m_weaponDataSO;

	private int m_currentAmmo;
	private bool m_isIdle;
	private float m_timer;

	private void Awake() {
		OnIdleStarted += LightningGun_OnIdleStarted;
		OnIdleEnded += LightningGun_OnIdleEnded;
	}

	protected virtual void Update() {
		if (!m_isIdle && m_timer > 0f && !shootingInput) {
			m_isIdle = true;
			OnIdleStarted?.Invoke(this, EventArgs.Empty);
		}
		HandleShooting();
	}

	private void HandleShooting() {
		m_timer -= Time.deltaTime;

		if (shootingInput && HasEnoughAmmo() && m_timer < 0f) {
			m_timer = m_weaponDataSO.rof / 1000f;

			Shoot();

			if (m_isIdle) {
				m_isIdle = false;
				OnIdleEnded?.Invoke(this, EventArgs.Empty);
			}
		}

	}

	private void Shoot() {
		Debug.Log("Shoot()"); // TODO remove
		OnShoot?.Invoke(this, EventArgs.Empty);
	}

	public override void SetAsCurrent() {
		base.SetAsCurrent();
		m_isIdle = true;
		OnIdleStarted?.Invoke(this, EventArgs.Empty);
	}

	private bool HasEnoughAmmo() {
		if (m_weaponDataSO.unlimitedAmmo) {
			return true;
		}
		return m_currentAmmo - 1 >= 0;
	}

	public override WeaponType GetWeaponType() {
		return WeaponType.LightningGun;
	}

	public override bool IsOnCooldown() {
		return false;
	}

	public void AddAmmo(int ammoAmount) {
		if (ammoAmount <= 0) {
			return;
		}
		m_currentAmmo = Mathf.Clamp(m_currentAmmo + ammoAmount, 0, m_weaponDataSO.maxAmmo);
	}

	public void AddStartingAmmo() {
		AddAmmo(m_weaponDataSO.startingAmmo);
	}

	public int GetStartingAmmo() {
		return m_weaponDataSO.startingAmmo;
	}

	private void LightningGun_OnIdleStarted(object sender, EventArgs e) {
		Debug.Log("LightningGun:OnIdleStarted()");
	}

	private void LightningGun_OnIdleEnded(object sender, EventArgs e) {
		Debug.Log("LightningGun:OnIdleEnded()");
	}
}
