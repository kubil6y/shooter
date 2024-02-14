using System;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : Singleton<ObjectPoolManager> {
	[SerializeField] private GameObject m_enemyDeathVFX;
	[SerializeField] private GameObject m_bulletHitVFX;

	[SerializeField] private Transform m_playerProjectileParentTf;
	[SerializeField] private Transform m_enemyDeathVFXParentTf;
	[SerializeField] private Transform m_bulletHitVFXParentTf;

	private ObjectPool<GameObject> m_enemyDeathVFXPool;
	private ObjectPool<GameObject> m_bulletHitVFXPool;

	private void Start() {
		m_enemyDeathVFXPool = CreateObjectPool(m_enemyDeathVFX, m_enemyDeathVFXParentTf, 5, 25);
		m_bulletHitVFXPool = CreateObjectPool(m_bulletHitVFX, m_bulletHitVFXParentTf, 20, 50);
	}

	public GameObject SpawnEnemyDeathVFX(Vector3 position) {
		GameObject enemyDeathVFX = m_enemyDeathVFXPool.Get();
		enemyDeathVFX.transform.position = position;
		return enemyDeathVFX;
	}

	public void RelaseEnemyDeathVFX(GameObject enemyDeathVFX) {
		m_enemyDeathVFXPool.Release(enemyDeathVFX);
	}


	public GameObject SpawnBulletHitVFX(Vector3 position) {
		GameObject bulletHitVFX = m_bulletHitVFXPool.Get();
		bulletHitVFX.transform.position = position;
		return bulletHitVFX;
	}

	public void RelaseBulletHitVFX(GameObject bulletHitVFX) {
		m_bulletHitVFXPool.Release(bulletHitVFX);
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
