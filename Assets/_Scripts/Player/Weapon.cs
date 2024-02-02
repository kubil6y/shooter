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
	public event EventHandler<State> OnStateChanged;
	public event EventHandler OnAmmoChanged;
	public event EventHandler OnOutOfAmmo;

	[SerializeField] protected WeaponDataSO weaponData;

	protected int m_ammo;

	public enum State {
		Idle,
		Fire,
		OnCooldown,
		OnOutOfAmmo,
	}

	protected bool isShooting;
	private float m_outOfAmmoWaitDuration = .4f;
	private float m_outOfAmmoWaitTimer;
	private float m_cooldownTimer;
	private State m_state = State.Idle;

	public abstract void Perform();

	protected virtual void Update() {
		switch (m_state) {
		case State.Idle:
			if (isShooting) {
				SetState(State.Fire);
			}
			break;

		case State.Fire:
			if (m_ammo - weaponData.ammoUsage >= 0) {
				m_ammo -= weaponData.ammoUsage;
				if (m_ammo < 0) {
					m_ammo = 0;
				}
				OnAmmoChanged?.Invoke(this, EventArgs.Empty);

				m_cooldownTimer = weaponData.rof / 1000f;
				Perform();
				SetState(State.OnCooldown);
			}
			else {
				m_outOfAmmoWaitTimer = m_outOfAmmoWaitDuration;
				OnOutOfAmmo?.Invoke(this, EventArgs.Empty);
				SetState(State.OnOutOfAmmo);
			}
			break;

		case State.OnCooldown:
			m_cooldownTimer -= Time.deltaTime;
			if (m_cooldownTimer < 0f) {
				if (!isShooting) {
					SetState(State.Idle);
					return;
				}
				if (weaponData.isSingleFire) {
					isShooting = false;
					SetState(State.Idle);
				}
				else {
					SetState(State.Fire);
				}
			}
			break;

		case State.OnOutOfAmmo:
			m_outOfAmmoWaitTimer -= Time.deltaTime;
			if (m_outOfAmmoWaitTimer < 0f) {
				SetState(State.Idle);
			}
			break;
		}
	}

	public WeaponDataSO GetWeaponDataSO() {
		return weaponData;
	}

	public int GetStartingAmmo() {
		return weaponData.startingAmmo;
	}

	public void AddAmmo(int amount) {
		m_ammo = Mathf.Clamp(m_ammo + amount, 0, weaponData.maxAmmo);
	}

	public void AddStartingAmmo() {
		m_ammo = weaponData.startingAmmo;
	}

	public bool IsOnCooldown() {
		return m_state == State.OnCooldown;
	}

	private void SetState(State state) {
		if (m_state == state) {
			return;
		}
		m_state = state;
		OnStateChanged?.Invoke(this, m_state);
	}

	public void Fire() {
		isShooting = true;
	}

	public void StopFiring() {
		isShooting = false;
	}

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}
}
