using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IHittable, IDamageable, IKnockable {
	public event EventHandler<OnHitEventArgs> OnHit;
	public class OnHitEventArgs : EventArgs {
		public float hitDuration;
		public WeaponType weaponType;
	}
	public event EventHandler<int> OnTakenDamage;
	public static event EventHandler OnAnyDeath;
	public int id { get; private set; }
	private static int s_id;

    [SerializeField] private Transform m_damagePopupTf;
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


    public Vector3 GetDamagePopupPosition() {
		return m_damagePopupTf.position;
    }

	public void TakeDamage(int damageAmount) {
		if (!health.IsAlive()) {
			return;
		}
		health.TakeDamage(damageAmount);
		OnTakenDamage?.Invoke(this, damageAmount);
	}

    public void TakeHit(WeaponType weaponType, float hitDuration) {
		OnHit?.Invoke(this, new OnHitEventArgs {
			hitDuration = hitDuration,
			weaponType = weaponType,
		});
    }

	public void GetKnocked(Vector3 hitDirection, float knockbackThrust, float knockbackDuration) {
		m_knockback.GetKnocked(Player.instance.transform.position, knockbackThrust, knockbackDuration);
	}

	private void Health_OnDeath(object sender, EventArgs e) {
		Destroy(gameObject);
	}
}
