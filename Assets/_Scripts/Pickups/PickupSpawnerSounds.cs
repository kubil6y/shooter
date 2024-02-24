using System;
using UnityEngine;

public class PickupSpawnerSounds : MonoBehaviour {
	private PickupSpawner m_pickupSpawner;

	private void Awake() {
		m_pickupSpawner = GetComponent<PickupSpawner>();
	}

	private void Start() {
		m_pickupSpawner.OnPickupSpawned += PickupSpawner_OnPickupSpawned;
	}

	private void PickupSpawner_OnPickupSpawned(object sender, EventArgs e) {
		AudioManager.instance.PlayPickupSpawned(transform.position);
	}
}
