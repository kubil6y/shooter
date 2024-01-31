using System;
using UnityEngine;

public enum WeaponType {
	Gauntlet,
	Pistol,
	MachineGun,
	LightningGun,
	RailGun,
	NailGun,
	Shotgun,
	RocketLauncher,
}

public abstract class Weapon : MonoBehaviour {
	public event EventHandler<State> OnStateChanged;

	[SerializeField] protected WeaponDataSO weaponData;

	public enum State {
		Idle,
		Fire,
		OnCooldown,
	}

	protected bool isShooting;
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
			m_cooldownTimer = weaponData.rof / 1000f;
			Perform();
			SetState(State.OnCooldown);
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
		}
	}

	private void SetState(State state) {
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
