using System;
using UnityEngine;

public class SoulDropper : MonoBehaviour {
	[SerializeField] private GameObject m_soulPrefab;
	[SerializeField] private int m_soulAmount = 1;
	[SerializeField] private float m_soulOffsetDistance = 1f;
	private Health m_health;

	private void Awake() {
		m_health = GetComponent<Health>();
	}

	private void Start() {
		m_health.OnDeath += Health_OnDeath;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.T)) {
			SpawnPrefab();
		}
	}

	private void SpawnPrefab() {
		if (m_soulAmount <= 0) {
			Debug.LogError($"Soul amount {m_soulAmount} is stupid. You are stupid.");
		}
		if (m_soulAmount == 1) {
			GameObject soulObject = Instantiate(m_soulPrefab);
			soulObject.transform.position = transform.position;
		}
		else {
			float spreadAngle = 360f / m_soulAmount;

			for (int i = 0; i < m_soulAmount; i++) {
				float angle = spreadAngle * i * Mathf.Deg2Rad;
				float xOffset = Mathf.Cos(angle) * m_soulOffsetDistance;
				float yOffset = Mathf.Sin(angle) * m_soulOffsetDistance;
				Vector3 offset = new Vector3(xOffset, yOffset, 0f);

				GameObject soulObject = Instantiate(m_soulPrefab);
				soulObject.transform.position = transform.position + offset;
			}
		}
	}

	private void Health_OnDeath(object sender, EventArgs e) {
		SpawnPrefab();
	}
}
