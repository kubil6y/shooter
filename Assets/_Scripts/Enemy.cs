using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable {
	[SerializeField] private float m_moveSpeed;
	private Movement m_movement;
	private Vector2 m_moveDirection;
	private Health m_health;

	private void Awake() {
		m_movement = GetComponent<Movement>();
		m_health = GetComponent<Health>();
	}

	private void Start() {
		m_health.OnDeath += Health_OnDeath;
	}

    private void Update() {
		// FollowPlayer();
		Debug.Log(m_health.GetCurrentHealth() + " " + m_health.GetCurrentArmor());
	}

	// private void FixedUpdate() {
	// 	m_movement.SetVelocity(m_moveSpeed * m_moveDirection);
	// }

	private void FollowPlayer() {
		m_moveDirection = (Player.instance.transform.position - transform.position).normalized;
	}

    public void TakeDamage(int damageAmount) {
		m_health.TakeDamage(damageAmount);
    }

    public void TakeHit() {
    }

    private void Health_OnDeath(object sender, EventArgs e) {
		Destroy(gameObject);
    }
}
