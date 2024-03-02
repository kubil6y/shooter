using System;
using UnityEngine;

public class BaseEnemyEffects : MonoBehaviour {
	[SerializeField] private Transform m_damagePopupTf;

	private BaseEnemy m_baseEnemy;
	private Flash m_flash;

	private void Awake() {
		m_baseEnemy = GetComponent<BaseEnemy>();
		m_flash = GetComponent<Flash>();
	}

	private void Start() {
		m_baseEnemy.health.OnDeath += Enemy_OnDeath;
		m_baseEnemy.OnTakenDamage += Enemy_OnTakenDamage;
		m_baseEnemy.OnHit += Enemy_OnHit;
	}

	private void Enemy_OnHit(object sender, Enemy.OnHitEventArgs e) {
		m_flash.StartFlash(e.hitDuration);
	}

	private void Enemy_OnDeath(object sender, EventArgs e) {
		ObjectPoolManager.instance.SpawnEnemyDeathVFX(transform.position);
	}

	private void Enemy_OnTakenDamage(object sender, int damageAmount) {
		ObjectPoolManager.instance.SpawnDamagePopup(m_damagePopupTf.position, damageAmount);
	}
}
