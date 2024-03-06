using UnityEngine;

public class EvilCrystal : MonoBehaviour {
	[SerializeField] private int m_damage;
	[SerializeField] private float m_moveSpeed;
	[SerializeField] private float m_lifetimeDuration = 5f;

	private Rigidbody2D m_rb;
	private Vector2 m_direction;
	private bool m_isAlive = true;
	private float m_lifetimer;

	private void Awake() {
		m_rb = GetComponent<Rigidbody2D>();
	}

	public void Setup(Vector2 direction) {
		m_lifetimer = 0f;
		m_isAlive = true;
		m_direction = direction;
	}

	private void Update() {
		m_lifetimer += Time.deltaTime;
		if (m_isAlive && m_lifetimer > m_lifetimeDuration) {
			m_isAlive = false;
			ObjectPoolManager.instance.ReleaseEvilCrystal(gameObject);
			return;
		}
	}

	private void FixedUpdate() {
		m_rb.velocity = m_direction * m_moveSpeed;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<Player>(out Player player)) {
			player.TakeHit(WeaponType.__LENGTH, hitDuration: .1f);
			player.TakeDamage(m_damage);
		}
		ObjectPoolManager.instance.ReleaseEvilCrystal(gameObject);
	}
}
