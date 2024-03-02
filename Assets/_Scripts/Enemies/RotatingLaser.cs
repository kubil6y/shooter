using System;
using UnityEngine;

public class RotatingLaser : MonoBehaviour {
	[SerializeField] private int m_damage = 50;
	[SerializeField] private float m_rotationSpeed = 100f;
	[SerializeField] private float m_damageBetweenDuration = .33f;

	private CapsuleCollider2D m_capsuleCollider;
	private float m_lastDamagedTime;

	private void Awake() {
		m_capsuleCollider = GetComponent<CapsuleCollider2D>();
	}

	private void Update() {
		HandleRotation();
	}

	private void HandleRotation() {
		transform.Rotate(Vector3.forward, m_rotationSpeed * Time.deltaTime);
	}

	private void OnTriggerStay2D(Collider2D other) {
		bool canDamagePlayer = Time.time - m_lastDamagedTime > m_damageBetweenDuration;
		if (!canDamagePlayer) {
			return;
		}
		if (other.TryGetComponent<Player>(out Player player)) {
			m_lastDamagedTime = Time.time;
			player.TakeDamage(m_damage);
			player.TakeHit(WeaponType.LightningGun, hitDuration: .1f);
		}
	}
}
