using System;
using UnityEngine;

public class LightningGun : Weapon {
	public event EventHandler OnShootStarted;
	public event EventHandler OnShootEnded;
	public event EventHandler OnIdleStarted;
	public event EventHandler OnIdleEnded;
	public event EventHandler OnOutOfAmmo;

	[SerializeField] private LightningGunWeaponDataSO m_weaponDataSO;
	[SerializeField] private Transform m_attackRefTf;
	[SerializeField] private LineRenderer m_lineRenderer;
	[SerializeField] private GameObject m_endFX;
	[SerializeField] private LayerMask m_laserLayerMask;

	private Transform m_weaponHolderTf;
	private float m_laserEndDistance;
	private float m_idleWaitDuration = .75f;
	private float m_idleTimer;

	private int m_currentAmmo;
	private float m_shootTimer;

	private bool m_isIdle;
	private bool m_isShooting;

	private void Awake() {
		m_endFX.SetActive(false);
	}

	private void Start() {
		m_weaponHolderTf = Player.instance.GetWeaponHolderTransform();
	}

	public override void SetAsCurrent() {
		base.SetAsCurrent();
		m_isIdle = true;
		m_idleTimer = 0f;
		OnIdleStarted?.Invoke(this, EventArgs.Empty);
	}

	private void Update() {
		m_shootTimer -= Time.deltaTime;
		m_idleTimer -= Time.deltaTime;

		if (shootInput && !HasEnoughAmmo()) {
			shootInput = false;
			OnOutOfAmmo?.Invoke(this, EventArgs.Empty);
		}
		else if (!m_isShooting && shootInput && HasEnoughAmmo()) {
			if (m_isIdle) {
				m_isIdle = false;
				OnIdleEnded?.Invoke(this, EventArgs.Empty);
			}
			m_isShooting = true;
			OnShootStarted?.Invoke(this, EventArgs.Empty);
		}

		if (m_isShooting && !shootInput) {
			m_isShooting = false;
			OnShootEnded?.Invoke(this, EventArgs.Empty);
		}
		else if (m_isShooting && !HasEnoughAmmo()) {
			m_isShooting = false;
			OnOutOfAmmo?.Invoke(this, EventArgs.Empty);
			OnShootEnded?.Invoke(this, EventArgs.Empty);
		}

		if (m_isShooting && m_shootTimer < 0f && HasEnoughAmmo()) {
			m_shootTimer = m_weaponDataSO.rof / 1000f;
			m_idleTimer = m_idleWaitDuration;
			m_currentAmmo--;
			Shoot();
		}

		if (!m_isShooting && !m_isIdle && m_idleTimer < 0f) {
			m_isIdle = true;
			OnIdleStarted?.Invoke(this, EventArgs.Empty);
		}

		if (m_isShooting) {
			LaserUpdate();
		}
	}

	private void LateUpdate() {
		if (m_isShooting) {
			m_lineRenderer.enabled = true;
		}
		else {
			m_lineRenderer.enabled = false;
			m_endFX.SetActive(false);
		}
	}

	private void LaserUpdate() {
		if (!m_weaponHolderTf) {
			return;
		}

		RaycastHit2D hit = Physics2D.Raycast(m_attackRefTf.position, m_weaponHolderTf.right, m_weaponDataSO.range, m_laserLayerMask);
		if (hit.collider != null) {
			m_laserEndDistance = Vector2.Distance(hit.point, m_attackRefTf.position);
			m_endFX.SetActive(true);
			m_endFX.transform.position = hit.point;
		}
		else {
			m_endFX.SetActive(false);
			m_laserEndDistance = m_weaponDataSO.range;
		}
		m_lineRenderer.SetPosition(1, new Vector2(m_laserEndDistance, 0f));
	}

	private void Shoot() {
		RaycastHit2D hit = Physics2D.Raycast(m_attackRefTf.position, m_weaponHolderTf.right, m_weaponDataSO.range, m_laserLayerMask);
		if (hit.collider != null) {
			float hitDuration = .05f;
			hit.collider.GetComponent<IHittable>()?.TakeHit(WeaponType.LightningGun, hitDuration);

			int damage = m_weaponDataSO.damagePerTick * weaponUser.GetDamageMultiplier();
			hit.collider.GetComponent<IDamageable>()?.TakeDamage(damage);

			hit.collider.GetComponent<IKnockable>()?.GetKnocked(Player.instance.transform.position, m_weaponDataSO.knockbackThrust, m_weaponDataSO.knockbackDuration);
		}
	}

	private bool HasEnoughAmmo() {
		if (m_weaponDataSO.unlimitedAmmo) {
			return true;
		}
		return m_currentAmmo - 1 >= 0;
	}

	public override bool IsOnCooldown() {
		return m_shootTimer > 0f;
	}

	public override void AddAmmo(int ammoAmount) {
		if (ammoAmount <= 0) {
			return;
		}
		m_currentAmmo = Mathf.Clamp(m_currentAmmo + ammoAmount, 0, m_weaponDataSO.maxAmmo);

		// OnAmmoChanged?.Invoke(this, EventArgs.Empty);
		Invoke_OnAmmoChanged();
	}

	public override void AddStartingAmmo() {
		AddAmmo(m_weaponDataSO.startingAmmo);
	}

	public override int GetStartingAmmo() {
		return m_weaponDataSO.startingAmmo;
	}

    public override int GetMaxAmmo() {
		return m_weaponDataSO.maxAmmo;
    }

	public override WeaponType GetWeaponType() {
		return WeaponType.LightningGun;
	}

	public override bool IsAvailable() {
		return HasEnoughAmmo();
	}

    public override int GetCurrentAmmo() {
		return m_currentAmmo;
    }

    public override bool HasUnlimitedAmmo() {
		return m_weaponDataSO.unlimitedAmmo;
    }
}
