using System;
using UnityEngine;

public class Projectile : MonoBehaviour {
	[SerializeField] private float m_moveSpeed = 10f;

	private Rigidbody2D m_rb;
	private ProjectileWeapon m_projectileWeapon;
	private Vector2 m_fireDirection;

	private void Awake() {
		m_rb = GetComponent<Rigidbody2D>();
	}

	public void Setup(ProjectileWeapon projectileWeapon, Vector2 spawnPos, Vector2 fireDirection) {
		m_projectileWeapon = projectileWeapon;
		transform.position = spawnPos;
		m_fireDirection = fireDirection;
	}

	private void FixedUpdate() {
		m_rb.velocity = m_fireDirection * m_moveSpeed;
	}

	protected virtual void OnCollisionEnter2D(Collision2D other) {
		m_projectileWeapon.ReleaseBulletFromPool(this);
	}
}
