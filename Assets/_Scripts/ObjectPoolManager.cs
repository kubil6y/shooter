using System;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : Singleton<ObjectPoolManager> {
	[SerializeField] private GameObject m_enemyDeathVFX;
	[SerializeField] private GameObject m_bulletHitVFX;
	[SerializeField] private GameObject m_bloodVFX;
	[SerializeField] private GameObject m_damagePopupPrefab;
	[SerializeField] private GameObject m_evilCrystalPrefab;

	[SerializeField] private Transform m_playerProjectileParentTf;
	[SerializeField] private Transform m_enemyDeathVFXParentTf;
	[SerializeField] private Transform m_bulletHitVFXParentTf;
	[SerializeField] private Transform m_bloodVFXParentTf;
	[SerializeField] private Transform m_damagePopupParentTf;
	[SerializeField] private Transform m_evilCrystalParentTf;

	private ObjectPool<GameObject> m_enemyDeathVFXPool;
	private ObjectPool<GameObject> m_bulletHitVFXPool;
	private ObjectPool<GameObject> m_bloodVFXPool;
	private ObjectPool<GameObject> m_damagePopupPool;
	private ObjectPool<GameObject> m_evilCrystalPool;

	private void Start() {
		CreatePools();
	}

	private void CreatePools() {
		m_enemyDeathVFXPool = CreateObjectPool(m_enemyDeathVFX, m_enemyDeathVFXParentTf, 5, 25);
		m_bulletHitVFXPool = CreateObjectPool(m_bulletHitVFX, m_bulletHitVFXParentTf, 20, 50);
		m_bloodVFXPool = CreateObjectPool(m_bloodVFX, m_bloodVFXParentTf, 5, 25);
		m_damagePopupPool = CreateObjectPool(m_damagePopupPrefab, m_damagePopupParentTf, 20, 50);
		m_evilCrystalPool = CreateObjectPool(m_evilCrystalPrefab, m_evilCrystalParentTf, 36, 72);
	}

	public GameObject SpawnEnemyDeathVFX(Vector3 position) {
		GameObject enemyDeathVFX = m_enemyDeathVFXPool.Get();
		enemyDeathVFX.transform.position = position;
		return enemyDeathVFX;
	}

	public void ReleaseEnemyDeathVFX(GameObject enemyDeathVFX) {
		m_enemyDeathVFXPool.Release(enemyDeathVFX);
	}

	public GameObject SpawnBulletHitVFX(Vector3 position) {
		GameObject bulletHitVFX = m_bulletHitVFXPool.Get();
		bulletHitVFX.transform.position = position;
		return bulletHitVFX;
	}

	public void ReleaseBulletHitVFX(GameObject bulletHitVFX) {
		m_bulletHitVFXPool.Release(bulletHitVFX);
	}

	public GameObject SpawnBloodVFX(Vector3 position) {
		GameObject bloodVFX = m_bloodVFXPool.Get();
		bloodVFX.transform.position = position;
		return bloodVFX;
	}

	public void ReleaseBloodVFX(GameObject bloodVFX) {
		m_bloodVFXPool.Release(bloodVFX);
	}

	public GameObject SpawnDamagePopup(Vector3 position, int damageAmount) {
		GameObject damagePopupObj = m_damagePopupPool.Get();
		damagePopupObj.transform.position = position;
		damagePopupObj.GetComponent<DamagePopup>()?.Setup(position, damageAmount);
		return damagePopupObj;
	}

	public void ReleaseDamagePopup(GameObject damagePopup) {
		m_damagePopupPool.Release(damagePopup);
	}

	public GameObject SpawnEvilCrystal(Vector3 position, Vector2 direction) {
		GameObject crystal = m_evilCrystalPool.Get();
		crystal.transform.position = position;
		crystal.GetComponent<EvilCrystal>()?.Setup(direction);
		return crystal;
	}

	public void ReleaseEvilCrystal(GameObject crystal) {
		m_evilCrystalPool.Release(crystal);
	}

	public Transform GetPlayerProjectileParentTransform() {
		return m_playerProjectileParentTf;
	}

	public ObjectPool<GameObject> CreateObjectPool(GameObject prefab, Transform parentTf, int defaultCapacity = 10, int maxSize = 50) {
		var pool = new ObjectPool<GameObject>(
			() => {
				GameObject obj = Instantiate(prefab);
				if (parentTf) {
					obj.transform.parent = parentTf;
				}
				return obj;
			},
			(obj) => {
				obj.gameObject.SetActive(true);
			},
			(obj) => {
				obj.gameObject.SetActive(false);
			},
			(obj) => {
				Destroy(obj);
			},
			false,
			defaultCapacity,
			maxSize);
		return pool;
	}
}
