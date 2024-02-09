using System;
using UnityEngine;

public class LightningGun : Weapon, IHasAmmo {
	public event EventHandler OnShoot;
	public event EventHandler OnAmmoChanged;
	public event EventHandler OnIdleStarted;
	public event EventHandler OnIdleEnded;

	[SerializeField] private LightningGunWeaponDataSO m_weaponDataSO;
	[SerializeField] private LineRenderer m_lineRenderer;
	[SerializeField] private Transform m_attackRefTf;

	private int m_currentAmmo;
	private float m_timer;
	private bool m_isIdle;

	private void Awake() {
		OnIdleStarted += LightningGun_OnIdleStarted;
		OnIdleEnded += LightningGun_OnIdleEnded;
	}

    private void Start() {
		DisableLaser();
	}

    private void DisableLaser() {
		// Debug.Log("DisableLaser()");
		m_lineRenderer.enabled = false;
    }

    private void EnableLaser() {
		// Debug.Log("EnableLaser()");
		m_lineRenderer.enabled = true;
    }

	private void UpdateLaser() {
		// m_lineRenderer.SetPosition(0, m_attackRefTf.position);
		// // TODO: i need to do a raycast in shoot and cache the hit position and then late update line renderer!
		// // https://www.youtube.com/watch?v=S6eRVwAtfOM at 10m
		// m_lineRenderer.SetPosition(1, m_attackRefTf.position);
	}

	private void FixedUpdate() {
		RaycastHit2D hit = Physics2D.Raycast(m_attackRefTf.position, transform.right, 4f);
	}

	protected virtual void Update() {
		if (!m_isIdle && m_timer > 0f && !shootingInput) {
			m_isIdle = true;
			OnIdleStarted?.Invoke(this, EventArgs.Empty);
		}
		if (shootingInput) {
			UpdateLaser();
		}
		HandleShooting();
	}

	private void HandleShooting() {
		m_timer -= Time.deltaTime;

		if (shootingInput && HasEnoughAmmo() && m_timer < 0f) {
			m_timer = m_weaponDataSO.rof / 1000f;

			if (m_isIdle) {
				m_isIdle = false;
				OnIdleEnded?.Invoke(this, EventArgs.Empty);
			}

			Shoot();
		}

	}

	private void Shoot() {
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
		DisableLaser();
	}

	private void LightningGun_OnIdleEnded(object sender, EventArgs e) {
		Debug.Log("LightningGun:OnIdleEnded()");
		EnableLaser();
	}
}
