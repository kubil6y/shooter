using System;
using UnityEngine;

[RequireComponent(typeof(Knockback))]
public class Movement : MonoBehaviour {
	private Rigidbody2D m_rb;
	private Vector2 m_velocity;
	private Knockback m_knockback;

	private bool m_canMove = true;

	private void Awake() {
		m_rb = GetComponent<Rigidbody2D>();
		m_knockback = GetComponent<Knockback>();
	}

	private void Start() {
		m_knockback.OnKnockbackStart += Knockback_OnKnockbackStart;
		m_knockback.OnKnockbackEnded += Knockback_OnKnockbackEnded;
	}

    private void FixedUpdate() {
		// TODO remove
		Debug.Log("canMove: " + m_canMove);
		Move();
	}

	private void Move() {
		if (!CanMove()) {
			return;
		}
		m_rb.velocity = m_velocity;
	}

	public void SetVelocity(Vector2 velocity) {
		m_velocity = velocity;
	}

	public void SetZeroVelocity() {
		SetVelocity(Vector2.zero);
	}

	public bool CanMove() {
		return m_canMove;
	}

	public void SetCanMove(bool canMove) {
		m_canMove = canMove;
	}

    private void Knockback_OnKnockbackStart(object sender, EventArgs e) {
		SetCanMove(false);
    }

    private void Knockback_OnKnockbackEnded(object sender, EventArgs e) {
		SetCanMove(true);
    }
}
