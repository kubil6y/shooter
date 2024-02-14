using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IHittable, IDamageable, IKnockable {
	public event EventHandler<float> OnHit;
	public static event EventHandler OnAnyDeath;
	public int id { get; private set; }
	private static int s_id;

	[SerializeField] private float m_moveSpeed;

	public Health health {get; private set; }

	private Movement m_movement;
	private Vector2 m_moveDirection;
	private Knockback m_knockback;
	private Flash m_flash;

	private void Awake() {
		m_movement = GetComponent<Movement>();
		m_knockback = GetComponent<Knockback>();
		health = GetComponent<Health>();
		m_flash = GetComponent<Flash>();
	}

	private void Start() {
		id = s_id++;
		health.OnDeath += Health_OnDeath;
		OnHit += Enemy_OnHit;
	}

    private void Update() {
		FollowPlayer();
	}

	private void FixedUpdate() {
		m_movement.SetVelocity(m_moveSpeed * m_moveDirection);
	}

	private void FollowPlayer() {
		m_moveDirection = (Player.instance.transform.position - transform.position).normalized;
	}

	public void TakeDamage(int damageAmount) {
		health.TakeDamage(damageAmount);
	}

    public void TakeHit(float hitDuration) {
		OnHit?.Invoke(this, hitDuration);
    }

	public void GetKnocked(Vector3 hitDirection, float knockbackThrust, float knockbackDuration) {
		m_knockback.GetKnocked(Player.instance.transform.position, knockbackThrust, knockbackDuration);
	}

	private void Health_OnDeath(object sender, EventArgs e) {
		Destroy(gameObject);
	}

    private void Enemy_OnHit(object sender, float duration) {
		m_flash.StartFlash(duration);
    }
}
