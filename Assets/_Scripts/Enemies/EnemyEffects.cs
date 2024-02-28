using System;
using UnityEngine;

public class EnemyEffects : MonoBehaviour {
	private Enemy m_enemy;
	private Flash m_flash;

	private void Awake() {
		m_enemy = GetComponent<Enemy>();
		m_flash = GetComponent<Flash>();
	}

	private void Start() {
		m_enemy.health.OnDeath += Enemy_OnDeath;
		m_enemy.OnTakenDamage += Enemy_OnTakenDamage;
		m_enemy.OnHit += Enemy_OnHit;
	}

	private void Enemy_OnHit(object sender, Enemy.OnHitEventArgs e) {
		m_flash.StartFlash(e.hitDuration);
	}

	private void Enemy_OnDeath(object sender, EventArgs e) {
		ObjectPoolManager.instance.SpawnEnemyDeathVFX(transform.position);
	}

	private void Enemy_OnTakenDamage(object sender, int damageAmount) {
		ObjectPoolManager.instance.SpawnDamagePopup(m_enemy.GetDamagePopupPosition(), damageAmount);
	}
}
