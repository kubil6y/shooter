using System;
using UnityEngine;
using DG.Tweening;

public class PickupSpawner : MonoBehaviour {
	public event EventHandler OnPickupSpawned;

	[SerializeField] private Pickup m_pickupPrefab;
	[SerializeField] private PickupSpawnerTimerUI m_pickupSpawnerTimerUI;

	private float m_spawnInterval = 5f;

	private float m_moveDistance = .25f;
	private float m_moveDuration = .75f;
	private Pickup m_pickup;
	private Tween m_pickupTween;

	private float m_spawnTimer;

	private void Start() {
		SpawnPickup();
		HideTimer();

		OnPickupSpawned += PickupSpawner_OnPickupSpawned;
	}

	private void Update() {
		if (!m_pickup) {
			m_spawnTimer -= Time.deltaTime;
		}
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

		OnPickupSpawned?.Invoke(this, EventArgs.Empty);
	}

	private void AttachAnimations() {
		if (!m_pickup) {
			return;
		}
		m_pickupTween = m_pickup.transform.DOMoveY(m_pickup.transform.position.y + m_moveDistance, m_moveDuration)
		  .SetEase(Ease.InOutQuad)
		  .SetLoops(-1, LoopType.Yoyo);
	}

	private void ShowTimer() {
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

		Invoke("SpawnPickup", m_spawnInterval);
	}

	private void PickupSpawner_OnPickupSpawned(object sender, EventArgs e) {
		// TODO remove
		Debug.Log(m_pickupPrefab.gameObject.name + " is spawned!");
	}
}
