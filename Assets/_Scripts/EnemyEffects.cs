using System;
using UnityEngine;

public class EnemyEffects : MonoBehaviour {
	private Enemy m_enemy;

	private void Awake() {
		m_enemy = GetComponent<Enemy>();
	}

	private void Start() {
		m_enemy.health.OnDeath += Enemy_OnDeath;
	}

    private void Enemy_OnDeath(object sender, EventArgs e) {
		ObjectPoolManager.instance.SpawnEnemyDeathVFX(transform.position);
    }
}
