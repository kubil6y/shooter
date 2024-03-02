using System;
using UnityEngine;

public class Demon : BaseEnemy {
	[SerializeField] private float m_moveSpeed = 4f;

	private DemonAnimations m_animations;

	public enum State {
		Chasing,
		Attack,
	}

	private Vector2 m_moveDir;
	private State m_state;

	protected override void Awake() {
		base.Awake();
		m_animations = GetComponentInChildren<DemonAnimations>();
	}

	protected override void Start() {
		base.Start();
		m_animations.OnAttacked += Demon_OnAttacked;
	}

	private void Update() {
		FollowPlayer();
	}

	private void FixedUpdate() {
		movement.SetVelocity(m_moveSpeed * m_moveDir);
	}

	private void FollowPlayer() {
		m_moveDir = (Player.instance.transform.position - transform.position).normalized;
	}

	private void Demon_OnAttacked(object sender, EventArgs e) {
		// TODO spawn some things!
		Debug.Log("Demon_OnAttacked()");
	}
}
