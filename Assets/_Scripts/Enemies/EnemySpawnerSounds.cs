using UnityEngine;

public class EnemySpawnerSounds : MonoBehaviour {
	private EnemySpawner m_enemySpawner;

	private void Awake() {
		m_enemySpawner = GetComponent<EnemySpawner>();
	}

	private void Start() {
		m_enemySpawner.OnEnemySpawned += EnemySpawner_OnEnemySpawned;
	}

	private void EnemySpawner_OnEnemySpawned(object sender, Vector2 spawnPosition) {
		AudioManager.instance.PlayEnemySpawned(spawnPosition);
	}
}
