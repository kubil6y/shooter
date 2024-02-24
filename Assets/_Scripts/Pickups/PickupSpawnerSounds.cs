using System;
using UnityEngine;

public class PickupSpawnerSounds : MonoBehaviour {
	[SerializeField] private bool m_hasSpawnSound = true;

	private PickupSpawner m_pickupSpawner;

	private void Awake() {
		m_pickupSpawner = GetComponent<PickupSpawner>();
	}

	private void Start() {
		m_pickupSpawner.OnPickupSpawned += PickupSpawner_OnPickupSpawned;
	}

	private void PickupSpawner_OnPickupSpawned(object sender, EventArgs e) {
		if (!m_hasSpawnSound) {
			return;
		}
		AudioManager.instance.PlayPickupSpawned(transform.position);
	}
}
