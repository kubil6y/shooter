using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	public event EventHandler OnEnemySpawned;

	[SerializeField] private Enemy m_enemyPrefab;
	[SerializeField] private int m_maxEnemyCount = 10;
	[SerializeField] private float m_spawnInterval = 1.5f;
	[SerializeField] private Transform[] m_spawnPoints;

	private float m_spawnTimer;
	private int m_enemyCount;

	private void Start() {
		Enemy.OnAnyDeath += Enemy_OnAnyDeath;
	}

    private void Update() {
		m_spawnTimer -= Time.deltaTime;
		if (m_spawnTimer < 0f) {
			m_spawnTimer = m_spawnInterval;
			SpawnEnemy();
		}
	}

	private void SpawnEnemy() {
		if (m_enemyCount >= m_maxEnemyCount) {
			return;
		}
		Enemy enemy = Instantiate(m_enemyPrefab, transform);
		Vector2 spawnPosition = GetSpawnPoint();
		enemy.transform.position = spawnPosition;

		OnEnemySpawned?.Invoke(this, EventArgs.Empty);
	}

	private Vector2 GetSpawnPoint() {
		if (m_spawnPoints.Length == 0) {
			return Vector2.zero;
		}
		int randomIndex = UnityEngine.Random.Range(0, m_spawnPoints.Length);
		return m_spawnPoints[randomIndex].position;
	}

    private void Enemy_OnAnyDeath(object sender, EventArgs e) {
		m_enemyCount--;
    }
}
