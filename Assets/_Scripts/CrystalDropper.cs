using System;
using UnityEngine;

public class CrystalDropper : MonoBehaviour {
	[SerializeField] private GameObject m_crystalPrefab;
	private Health m_health;

	private void Awake() {
		m_health = GetComponent<Health>();
	}

	private void Start() {
		m_health.OnDeath += Health_OnDeath;
	}

	private void SpawnPrefab() {
		GameObject crystalObject = Instantiate(m_crystalPrefab);
		crystalObject.transform.position = transform.position;
	}

	private void Health_OnDeath(object sender, EventArgs e) {
		SpawnPrefab();
	}
}
