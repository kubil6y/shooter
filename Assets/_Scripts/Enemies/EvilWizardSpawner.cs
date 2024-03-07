using System;
using UnityEngine;

public class EvilWizardSpawner : MonoBehaviour {
	public event EventHandler<Vector2> OnEnemySpawned;

	[SerializeField] private EvilWizard m_evilWizard;
	[SerializeField] private EnemySpawnPortal m_spawnPortal;
	[SerializeField] private Transform[] m_spawnPoints;
	[SerializeField] private float m_spawnInterval = 5f;

	private enum SpawnPosition {
		Left,
		Right,
	}

	private SpawnPosition m_spawnPosition = SpawnPosition.Right;
	private bool m_canSpawn;
	private bool m_hasSpawned;
	private float m_spawnTimer;

	private void Start() {
		if (m_spawnPoints.Length != 2) {
			Debug.LogError("EvilWizardSpawner: Fix spawn points!");
		}
		GameManager.instance.OnPlayingStarted += GameManager_OnPlayingStarted;
		GameManager.instance.OnGameOver += GameManager_OnGameOver;
		EvilWizard.OnAnyEvilWizardDeath += EvilWizard_OnAnyEvilWizardDeath;
	}

	private void Update() {
		m_spawnTimer -= Time.deltaTime;
		if (m_canSpawn && !m_hasSpawned && m_spawnTimer < 0f) {
			SpawnEvilWizard();
		}
	}

	private void SpawnEvilWizard() {
		m_hasSpawned = true;
		Vector2 spawnPosition = GetNextSpawnPosition();
		EnemySpawnPortal spawnPortal = Instantiate(m_spawnPortal, transform);
		spawnPortal.transform.position = spawnPosition;

		spawnPortal.Setup(() => {
			EvilWizard wizard = UnityEngine.GameObject.Instantiate<EvilWizard>(m_evilWizard, transform);
			wizard.transform.position = spawnPortal.GetSpawnPoint();
		});

		OnEnemySpawned?.Invoke(this, spawnPosition);
	}

	private Vector2 GetNextSpawnPosition() {
		switch (m_spawnPosition) {
		case SpawnPosition.Left:
			m_spawnPosition = SpawnPosition.Right;
			return GetSpawnPosition(SpawnPosition.Right);
		default:
		case SpawnPosition.Right:
			m_spawnPosition = SpawnPosition.Left;
			return GetSpawnPosition(SpawnPosition.Left);
		}
	}

	private Vector2 GetSpawnPosition(SpawnPosition spawnPosition) {
		switch (spawnPosition) {
		case SpawnPosition.Left:
			return m_spawnPoints[0].position;
		default:
		case SpawnPosition.Right:
			return m_spawnPoints[1].position;
		}
	}

	private void GameManager_OnPlayingStarted(object sender, EventArgs e) {
		m_canSpawn = true;
	}

	private void GameManager_OnGameOver(object sender, EventArgs e) {
		m_canSpawn = false;
	}

	private void EvilWizard_OnAnyEvilWizardDeath(object sender, EventArgs e) {
		m_spawnTimer = m_spawnInterval;
		m_hasSpawned = false;
	}
}
