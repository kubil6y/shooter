using System;
using UnityEngine;

public class EnemyEffects : MonoBehaviour {
	private Enemy m_enemy;
	private float m_lastHitTime;

	private void Awake() {
		m_enemy = GetComponent<Enemy>();
	}

	private void Start() {
		m_enemy.health.OnDeath += Enemy_OnDeath;
		m_enemy.OnTakenDamage += Enemy_OnTakenDamage;
	}

	private void Enemy_OnDeath(object sender, EventArgs e) {
		ObjectPoolManager.instance.SpawnEnemyDeathVFX(transform.position);
	}

	private void Enemy_OnTakenDamage(object sender, int damageAmount) {
		ObjectPoolManager.instance.SpawnDamagePopup(m_enemy.GetDamagePopupPosition(), damageAmount);
	}

}
