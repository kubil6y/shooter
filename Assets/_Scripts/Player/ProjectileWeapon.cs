using UnityEngine;
using UnityEngine.Pool;

public class ProjectileWeapon : Weapon {
	[SerializeField] protected Transform m_muzzleTf;

	private ObjectPool<Projectile> m_projectilePool;
	private Transform m_projectileObjectPoolParentTf;

	private void Start() {
		CreateBulletPool();
	}

	public void Setup(Transform objectPoolsTf) {
		if (objectPoolsTf != null) {
			Debug.Log("here here !");
			GameObject newPoolParent = new GameObject(weaponData.weaponName + " Pool");
			newPoolParent.transform.parent = objectPoolsTf;
			m_projectileObjectPoolParentTf = newPoolParent.transform;
		}
	}

	public override void Perform() {
		Projectile newProjectile = m_projectilePool.Get();

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0f;
		Vector2 fireDirection = (mousePos - m_muzzleTf.position).normalized;

		newProjectile.Setup(this, m_muzzleTf.position, fireDirection);
	}

	private void CreateBulletPool() {
		if (weaponData.poolSize <= 0) {
			Debug.LogError($"Pool size error '{weaponData.poolSize}' is invalid.");
			return;
		}

		m_projectilePool = new ObjectPool<Projectile>(() => {
			Projectile projectile = Instantiate(weaponData.projectilePrefab);
			if (m_projectileObjectPoolParentTf != null) {
				projectile.transform.parent = m_projectileObjectPoolParentTf;
			}
			return projectile;
		}, (projectile) => {
			projectile.gameObject.SetActive(true);
		}, (projectile) => {
			projectile.gameObject.SetActive(false);
		}, (projectile) => {
			Destroy(projectile);
		}, false, weaponData.poolSize, weaponData.poolSize * 2);
	}

	public void ReleaseBulletFromPool(Projectile projectile) {
		m_projectilePool.Release(projectile);
	}
}
