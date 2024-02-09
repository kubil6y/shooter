using System;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileWeapon : Weapon, IHasAmmo, IHasObjectPool {
	public event EventHandler OnShoot;
	public event EventHandler OnAmmoChanged;
	public event EventHandler OnIdleStarted;
	public event EventHandler OnIdleEnded;

	[SerializeField] private ProjectileWeaponDataSO m_weaponDataSO;
	[SerializeField] private Transform m_muzzleTf;

	private Transform m_projectileObjectPoolParentTf;
	private ObjectPool<Projectile> m_projectilePool;

	private int m_currentAmmo;
	private bool m_isIdle;
	private float m_timer;

	private void Awake() {
		CreateProjectilePool();

		// TODO remove these later.
		OnIdleStarted += ProjectileWeapon_OnIdleStarted;
		OnIdleEnded += ProjectileWeapon_OnIdleEnded;
	}

	protected virtual void Update() {
		if (!m_isIdle && m_timer > 0f && !shootingInput) {
			m_isIdle = true;
			OnIdleStarted?.Invoke(this, EventArgs.Empty);
		}
		HandleShooting();
	}

	public override void SetAsCurrent() {
		base.SetAsCurrent();
		m_isIdle = true;
		OnIdleStarted?.Invoke(this, EventArgs.Empty);
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

			if (m_weaponDataSO.singleFire) {
				shootingInput = false;
			}
		}

	}

	private void Shoot() {
		for (int i = 0; i < m_weaponDataSO.ammoUsage; i++) {
			Projectile newProjectile = m_projectilePool.Get();

			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = 0f;
			Vector2 fireDirection = (mousePos - m_muzzleTf.position).normalized;

			if (m_weaponDataSO.spreadAngle != 0) {
				float spreadAngle = UnityEngine.Random.Range(-m_weaponDataSO.spreadAngle, m_weaponDataSO.spreadAngle);
				// Rotate fireDirection by spreadAngle
				Quaternion rotation = Quaternion.Euler(0, 0, spreadAngle);
				fireDirection = rotation * fireDirection;
			}

			// TODO: requires knockback settings...
			var projectileSetupArgs = new Projectile.ProjectileSetupArgs {
				projectileWeapon = this,
				spawnTf = m_muzzleTf,
				fireDirection = fireDirection,
				moveSpeed = m_weaponDataSO.projectileSpeed,
				damage = m_weaponDataSO.projectileDamage,
				lifetime = 3f,
			};

			newProjectile.Setup(projectileSetupArgs);
		}

		m_currentAmmo -= m_weaponDataSO.ammoUsage;
		OnShoot?.Invoke(this, EventArgs.Empty);
		OnAmmoChanged?.Invoke(this, EventArgs.Empty);
	}

	private bool HasEnoughAmmo() {
		if (m_weaponDataSO.unlimitedAmmo) {
			return true;
		}
		return m_currentAmmo - m_weaponDataSO.ammoUsage >= 0;
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

	public override WeaponType GetWeaponType() {
		return m_weaponDataSO.weaponType;
	}

	public override bool IsOnCooldown() {
		return m_timer > 0f;
	}

	public void SetupObjectPoolParent(Transform objectPoolsParentTf) {
		if (!objectPoolsParentTf) {
			Debug.LogWarning($"'{gameObject.name}' SetupObjectPool has failed.");
			return;
		}
		GameObject newPoolParent = new GameObject(m_weaponDataSO.weaponName + " Pool");
		newPoolParent.transform.parent = objectPoolsParentTf;
		m_projectileObjectPoolParentTf = newPoolParent.transform;
	}

	public void ReleaseProjectileFromPool(Projectile projectile) {
		m_projectilePool.Release(projectile);
	}

	private void CreateProjectilePool() {
		if (m_weaponDataSO.poolSize <= 0) {
			Debug.LogWarning($"Pool size error '{m_weaponDataSO.poolSize}' is invalid.");
			return;
		}

		m_projectilePool = new ObjectPool<Projectile>(
			() => {
				Projectile projectile = Instantiate(m_weaponDataSO.projectilePrefab);
				if (m_projectileObjectPoolParentTf) {
					projectile.transform.parent = m_projectileObjectPoolParentTf;
				}
				return projectile;
			},
			(projectile) => {
				projectile.gameObject.SetActive(true);
			},
			(projectile) => {
				projectile.gameObject.SetActive(false);
			},
			(projectile) => {
				Destroy(projectile);
			},
			false,
			m_weaponDataSO.poolSize,
			m_weaponDataSO.poolSize * 2);
	}

	private void ProjectileWeapon_OnIdleStarted(object sender, EventArgs e) {
		Debug.Log(gameObject.name + ":OnIdleStarted()");
	}

	private void ProjectileWeapon_OnIdleEnded(object sender, EventArgs e) {
		Debug.Log(gameObject.name + ":OnIdleEnded()");
	}
}
