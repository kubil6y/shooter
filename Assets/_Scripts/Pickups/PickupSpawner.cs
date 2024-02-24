using System;
using UnityEngine;
using DG.Tweening;

public class PickupSpawner : MonoBehaviour {
	public event EventHandler OnPickupSpawned;

	[SerializeField] private Pickup m_pickupPrefab;
	[SerializeField] private PickupSpawnerTimerUI m_pickupSpawnerTimerUI;
	[SerializeField] private bool m_showTimerUI = true;
	[SerializeField] private bool m_hasAnimation = true;
	[SerializeField] private bool m_fireOnInitialSpawn;

	private float m_moveDistance = .5f;
	private float m_moveDuration = .5f;
	private Pickup m_pickup;
	private Tween m_pickupTween;

	private float m_spawnTimer;
	private float m_spawnInterval;
	private float m_initialSpawnTimer;
	private bool m_canSpawn;
	private bool m_initialSpawn = true;

	private void Start() {
		m_initialSpawnTimer = m_pickupPrefab.GetInitialSpawnDelay();
		m_spawnInterval = m_pickupPrefab.GetPickupSpawnInterval();
		HideTimer();
	}

	private void Update() {
		HandleInitialSpawn();
		HandleSpawning();
	}

	private void HandleInitialSpawn() {
		m_initialSpawnTimer -= Time.deltaTime;
		if (m_initialSpawnTimer < 0f && !m_canSpawn) {
			m_canSpawn = true;
		}
	}

	private void HandleSpawning() {
		if (!m_canSpawn) {
			return;
		}

		m_spawnTimer -= Time.deltaTime;
		if (m_spawnTimer < 0f) {
			m_spawnTimer = m_spawnInterval;
			SpawnPickup();
		}
	}

	public float GetSpawnTimer() {
		return m_spawnTimer;
	}

	public float GetSpawnTimerNormalized() {
		return 1f - m_spawnTimer / m_spawnInterval;
	}

	private void SpawnPickup() {
		if (m_pickup) {
			return;
		}

		m_pickup = Instantiate(m_pickupPrefab, transform);
		m_pickup.OnPickedUp += Pickup_OnPickedUp;

		AttachAnimations();
		HideTimer();

		if (m_initialSpawn) {
			m_initialSpawn = false;
			if (!m_fireOnInitialSpawn) {
				return;
			}
		}

		OnPickupSpawned?.Invoke(this, EventArgs.Empty);
	}

	private void AttachAnimations() {
		if (!m_pickup) {
			return;
		}
		if (!m_hasAnimation) {
			return;
		}
		m_pickupTween = m_pickup.transform.DOMoveY(m_pickup.transform.position.y + m_moveDistance, m_moveDuration)
		  .SetEase(Ease.InOutQuad)
		  .SetLoops(-1, LoopType.Yoyo);
	}

	private void ShowTimer() {
		if (!m_showTimerUI) {
			return;
		}
		m_pickupSpawnerTimerUI.Show();
	}

	private void HideTimer() {
		m_pickupSpawnerTimerUI.Hide();
	}

	private void Pickup_OnPickedUp(object sender, EventArgs e) {
		m_pickup.OnPickedUp -= Pickup_OnPickedUp;
		m_pickupTween.Kill();

		ShowTimer();
		m_spawnTimer = m_spawnInterval;

		// Invoke("SpawnPickup", m_spawnInterval);
	}
}
