using System;
using UnityEngine;

public class Enemy : BaseEnemy {
	[SerializeField] private float m_moveSpeed;

	private Vector2 m_moveDir;

	private void Update() {
		FollowPlayer();
	}

	private void FixedUpdate() {
		movement.SetVelocity(m_moveSpeed * m_moveDir);
	}

	private void FollowPlayer() {
		m_moveDir = (Player.instance.transform.position - transform.position).normalized;
	}
}