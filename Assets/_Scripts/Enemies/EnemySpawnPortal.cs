using System;
using UnityEngine;

public class EnemySpawnPortal : MonoBehaviour {
	[SerializeField] private Transform m_spawnPointTf;
	private Action m_callback;

	public void Setup(Action callback) {
		m_callback = callback;
	}

	public Vector2 GetSpawnPoint() {
		return m_spawnPointTf.position;
	}

	private void AnimEndTrigger() {
		m_callback();
		Destroy(gameObject);
	}
}
