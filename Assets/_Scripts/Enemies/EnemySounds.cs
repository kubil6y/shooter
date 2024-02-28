using System;
using UnityEngine;

public class EnemySounds : MonoBehaviour {
	private Enemy m_enemy;

	private void Awake() {
		m_enemy = GetComponent<Enemy>();
	}

	private void Start() {
		m_enemy.health.OnDeath += Enemy_OnDeath;
		m_enemy.OnHit += Enemy_OnHit;
	}

	private void Enemy_OnDeath(object sender, EventArgs e) {
		AudioManager.instance.PlayEnemyImplosion(transform.position);
	}

	private void Enemy_OnHit(object sender, Enemy.OnHitEventArgs e) {
		switch (e.weaponType) {
		case WeaponType.Chainsaw:
			AudioManager.instance.PlayChainsawHit(transform.position);
			break;
		case WeaponType.LightningGun:
			AudioManager.instance.PlayLGHits(transform.position);
			break;
		default:
			AudioManager.instance.PlayHitmarker(transform.position);
			break;
		}
	}
}
