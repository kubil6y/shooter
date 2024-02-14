using UnityEngine;
using UnityEngine.Pool;

public class VFXManager : Singleton<VFXManager> {
	[SerializeField] private ParticleSystem m_enemyDeathVFX;

	private ObjectPool<ParticleSystem> m_enemyDeathVFXPool;

	private void Start() {
		CreateEnemyDeathVFXPool();
	}

	public ParticleSystem SpawnEnemyDeathVFX(Vector3 position) {
		ParticleSystem enemyDeathVFX = m_enemyDeathVFXPool.Get();
		enemyDeathVFX.transform.position = position;
		return enemyDeathVFX;
	}

	public void RelaseEnemyDeathVFX(ParticleSystem enemyDeathVFX) {
		m_enemyDeathVFXPool.Release(enemyDeathVFX);
	}

	private void CreateEnemyDeathVFXPool() {
		m_enemyDeathVFXPool = new ObjectPool<ParticleSystem>(
			() => {
				ParticleSystem enemyDeathVFX = Instantiate(m_enemyDeathVFX);
				Transform enemyDeathVFXParentTf = ObjectPoolManager.instance.GetEnemyDeathVFXParentTransform();
				if (enemyDeathVFXParentTf) {
					enemyDeathVFX.transform.parent = enemyDeathVFXParentTf;
				}
				return enemyDeathVFX;
			},
			(enemyDeathVFX) => {
				enemyDeathVFX.gameObject.SetActive(true);
			},
			(enemyDeathVFX) => {
				enemyDeathVFX.gameObject.SetActive(false);
			},
			(enemyDeathVFX) => {
				Destroy(enemyDeathVFX);
			},
			false,
			10,
			50);
	}
}
