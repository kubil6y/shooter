using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	private float m_moveSpeed = 10f;
	private Rigidbody2D m_rb;
	private ProjectileWeapon m_projectileWeapon;
	private Vector2 m_fireDirection;
	private int m_damage;
	private float m_lifetimer;
	private float m_knockbackThrust;
	private float m_knockbackDuration;
	private bool m_canGoThrough;
	private int m_projectileGoThroughCount;
	private LayerMask m_targetLayerMask; // TODO handle target layer mask

	private List<int> m_wentThroughEnemies; // needs to be reset on object pool

	// TODO handle args for gc
	public class ProjectileSetupArgs {
		public ProjectileWeapon projectileWeapon;
		public Transform spawnTf;
		public Vector2 fireDirection;
		public float moveSpeed;
		public int damage;
		public float lifetime;
		public LayerMask targetLayerMask;
		public float knockbackThrust;
		public float knockbackDuration;
		public bool projectileCanGoThrough;
		public int projectileGoThroughCount;
	}

	private void Awake() {
		m_rb = GetComponent<Rigidbody2D>();
		m_wentThroughEnemies = new List<int>();
	}

	public void Setup(ProjectileSetupArgs args) {
		m_wentThroughEnemies.Clear();
		m_projectileWeapon = args.projectileWeapon;
		transform.position = args.spawnTf.position;
		m_fireDirection = args.fireDirection;
		m_moveSpeed = args.moveSpeed;
		m_damage = args.damage;
		m_lifetimer = args.lifetime;
		m_targetLayerMask = args.targetLayerMask;
		m_knockbackThrust = args.knockbackThrust;
		m_knockbackDuration = args.knockbackDuration;
		m_canGoThrough = args.projectileCanGoThrough;
		m_projectileGoThroughCount = args.projectileGoThroughCount;

		transform.right = m_fireDirection; // TODO deneme
	}

	private void Update() {
		m_lifetimer -= Time.deltaTime;
		if (m_lifetimer < 0f) {
			m_projectileWeapon.ReleaseProjectileFromPool(this);
		}
	}

	private void FixedUpdate() {
		m_rb.velocity = m_fireDirection * m_moveSpeed;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// if (((1 << other.gameObject.layer) & m_targetLayerMask) == 0) {
		// 	return;
		// }

		IHittable hittable = other.GetComponent<IHittable>();
		float hitDuration = .1f;
		hittable?.TakeHit(hitDuration);

		IDamageable damageable = other.GetComponent<IDamageable>();
		damageable?.TakeDamage(m_damage);

		IKnockable knockable = other.GetComponent<IKnockable>();
		knockable?.GetKnocked(transform.position, m_knockbackThrust, m_knockbackDuration);

		ObjectPoolManager.instance.SpawnBulletHitVFX(transform.position);

		if (m_canGoThrough) {
			if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy)) {
				if (!m_wentThroughEnemies.Contains(enemy.id)) {
					m_wentThroughEnemies.Add(enemy.id);
				}

				if (m_wentThroughEnemies.Count >= m_projectileGoThroughCount) {
					Debug.Log(m_wentThroughEnemies.Count);
					m_projectileWeapon.ReleaseProjectileFromPool(this);
					return;
				}
			}
		}
		else {
			m_projectileWeapon.ReleaseProjectileFromPool(this);
		}

	}
}
