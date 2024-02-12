using System;
using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour {
	public event EventHandler OnKnockbackStart;
	public event EventHandler OnKnockbackEnded;

	private Rigidbody2D m_rb;

	private Vector3 m_hitDirection;
	private float m_knockbackThrust;
	private float m_knockbackDuration;
	private Coroutine m_knockbackRoutine;
	private bool m_canGetKnocked = true;

	private void Awake() {
		m_rb = GetComponent<Rigidbody2D>();
	}

	public bool CanGetKnocked() {
		return m_canGetKnocked;
	}

	public void SetCanGetKnocked(bool canGetKnocked) {
		m_canGetKnocked = canGetKnocked;
	}

	public void GetKnocked(Vector3 hitDirection, float knockbackThrust, float knockbackDuration) {
		if (!CanGetKnocked()) {
			return;
		}
		if (knockbackThrust == 0) {
			return;
		}

		m_hitDirection = hitDirection;
		m_knockbackThrust = knockbackThrust;
		m_knockbackDuration = knockbackDuration;
		ApplyKnockbackForce();
	}

	private void ApplyKnockbackForce() {
		Vector3 difference = (transform.position - m_hitDirection).normalized * m_knockbackThrust * m_rb.mass;
		m_rb.AddForce(difference, ForceMode2D.Impulse);

		if (m_knockbackRoutine == null) {
			m_knockbackRoutine = StartCoroutine(KnockbackRoutine());
		}
		else {
			// End previous knockback routine and start new one.
			OnKnockbackEnded?.Invoke(this, EventArgs.Empty);
			StopCoroutine(m_knockbackRoutine);
			m_knockbackRoutine = StartCoroutine(KnockbackRoutine());
		}

	}

	private IEnumerator KnockbackRoutine() {
		OnKnockbackStart?.Invoke(this, EventArgs.Empty);
		yield return new WaitForSeconds(m_knockbackDuration);
		OnKnockbackEnded?.Invoke(this, EventArgs.Empty);
	}
}
