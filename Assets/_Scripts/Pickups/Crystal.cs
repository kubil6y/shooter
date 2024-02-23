using System;
using UnityEngine;

public class Crystal : MonoBehaviour {
	public event EventHandler OnCrystalLifetimeEnded;

	[SerializeField] private float m_followDistance = 6f;
	[SerializeField] private float m_moveSpeed = 3f;
	[SerializeField] private float m_lifetimeDuration = 8f;

	private float m_lifetimer;

	private Rigidbody2D m_rb;
	private Transform m_playerTf;
	private bool m_isAlive = true;

	private void Awake() {
		m_rb = GetComponent<Rigidbody2D>();
	}

	private void Start() {
		m_playerTf = Player.instance.transform;
		m_lifetimer = m_lifetimeDuration;
	}

	private void Update() {
		m_lifetimer -= Time.deltaTime;
		if (m_isAlive && m_lifetimer < 0f) {
			m_isAlive = false;
			OnCrystalLifetimeEnded?.Invoke(this, EventArgs.Empty);
		}
	}

	private void FixedUpdate() {
		if (!m_playerTf) {
			return;
		}

		float distanceToPlayer = Vector2.Distance(m_playerTf.position, transform.position);
		float multiplier = distanceToPlayer < m_followDistance ? 1 : 0;
		Vector2 dirToPlayer = (m_playerTf.position - transform.position).normalized;
		m_rb.velocity = dirToPlayer * m_moveSpeed * multiplier;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<Player>(out Player player)) {
			player.TryCollectCrystal();
			Destroy(gameObject);
		}
	}
}
