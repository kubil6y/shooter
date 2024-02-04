using UnityEngine;
using UnityEngine.Pool;

public class ProjectileWeapon : Weapon, IHasAmmo, IHasObjectPool {
	[SerializeField] private ProjectileWeaponDataSO m_weaponDataSO;
	[SerializeField] private Transform m_muzzleTf;

	private Transform m_projectileObjectPoolParentTf;
	private ObjectPool<Projectile> m_projectilePool;

	private int m_currentAmmo;
	private float m_timer;

	private void Start() {
		CreateProjectilePool();
	}

	private void Update() {
		m_timer -= Time.deltaTime;

		if (shootingInput && m_timer < 0f) {
			m_timer = .1f;
			Fire();
		}
	}

	private void Fire() {
		Projectile newProjectile = m_projectilePool.Get();

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0f;
		Vector2 fireDirection = (mousePos - m_muzzleTf.position).normalized;

		newProjectile.Setup(this, m_muzzleTf.position, fireDirection, m_weaponDataSO.projectileSpeed);
	}

	private bool HasEnoughAmmo() {
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
		// TODO
		return false;
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
}
