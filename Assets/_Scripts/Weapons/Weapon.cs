using System;
using UnityEngine;

// NOTE: WeaponType manages index positions and size in the WeaponManager.m_weaponArray
public enum WeaponType {
	Gauntlet,
	Pistol,
	MachineGun,
	LightningGun,
	RailGun,
	NailGun,
	Shotgun,
	RocketLauncher,
	__LENGTH,
}

public abstract class Weapon : MonoBehaviour {
	public event EventHandler OnAmmoChanged;
	public event EventHandler OnOutOfAmmo;
	public event EventHandler OnFired;

	[SerializeField] protected WeaponDataSO weaponData;

	protected int ammo;
	protected bool shootingInput;
	private float m_outOfAmmoWaitDuration = .3f;
	private float m_timer;

	private bool m_isFiring;

	public abstract void Perform();

	private void Update() {
		m_timer -= Time.deltaTime;

		bool hasEnoughAmmo = ammo - weaponData.ammoUsage >= 0;
		m_isFiring = hasEnoughAmmo && shootingInput;

		if (shootingInput && m_timer < 0f) {
			if (hasEnoughAmmo) {
				ammo = Mathf.Clamp(ammo - weaponData.ammoUsage, 0, weaponData.maxAmmo);
				OnAmmoChanged?.Invoke(this, EventArgs.Empty);

				m_timer = weaponData.rof / 1000f;
				Perform();
				OnFired?.Invoke(this, EventArgs.Empty);
				if (weaponData.isSingleFire) {
					shootingInput = false;
				}
			}
			else {
				m_timer = m_outOfAmmoWaitDuration;
				shootingInput = false;
				OnOutOfAmmo?.Invoke(this, EventArgs.Empty);
			}

		}
	}

	public void Fire() {
		shootingInput = true;
	}

	public void StopFiring() {
		shootingInput = false;
	}

	// IsFiring will be used for LG
	public bool IsFiring() {
		return m_isFiring;
	}

	public bool GetIsIdle() {
		return !shootingInput && m_timer < 0f;
	}


	public WeaponDataSO GetWeaponDataSO() {
		return weaponData;
	}

	public int GetStartingAmmo() {
		return weaponData.startingAmmo;
	}

	public void AddAmmo(int amount) {
		ammo = Mathf.Clamp(ammo + amount, 0, weaponData.maxAmmo);
	}

	public void AddStartingAmmo() {
		ammo = weaponData.startingAmmo;
	}

	public bool IsOnCooldown() {
		return m_timer > 0f;
	}

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}
}
