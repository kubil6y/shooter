using System;
using UnityEngine;

public class SoulDropper : MonoBehaviour {
	[SerializeField] private GameObject m_soulPrefab;
	private Health m_health;

	private void Awake() {
		m_health = GetComponent<Health>();
	}

	private void Start() {
		m_health.OnDeath += Health_OnDeath;
	}

	private void SpawnPrefab() {
		GameObject soulObject = Instantiate(m_soulPrefab);
		soulObject.transform.position = transform.position;
	}

	private void Health_OnDeath(object sender, EventArgs e) {
		SpawnPrefab();
	}
}
