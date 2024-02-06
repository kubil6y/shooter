using UnityEngine;

public class Projectile : MonoBehaviour {
	private float m_moveSpeed = 10f;
	private Rigidbody2D m_rb;
	private ProjectileWeapon m_projectileWeapon;
	private Vector2 m_fireDirection;
	private int m_damage;
	private float m_lifetimer;

	// TODO: requires knockback settings...
	public class ProjectileSetupArgs {
		public ProjectileWeapon projectileWeapon;
		public Transform spawnTf;
		public Vector2 fireDirection;
		public float moveSpeed;
		public int damage;
		public float lifetime;
	}

	private void Awake() {
		m_rb = GetComponent<Rigidbody2D>();
	}

	public void Setup(ProjectileSetupArgs args) {
		m_projectileWeapon = args.projectileWeapon;
		transform.position = args.spawnTf.position;
		m_fireDirection = args.fireDirection;
		m_moveSpeed = args.moveSpeed;
		m_damage = args.damage;
		m_lifetimer = args.lifetime;
	}

	private void Update() {
		m_lifetimer -= Time.deltaTime;
		if (m_lifetimer < 0f) {
			m_projectileWeapon.ReleaseProjectileFromPool(this);
		}
	}

	private void FixedUpdate() {
		m_rb.velocity = m_fireDirection * m_moveSpeed;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		IHittable hittable = other.GetComponent<IHittable>();
		hittable?.TakeHit();

		IDamageable damageable = other.GetComponent<IDamageable>();
		damageable?.TakeDamage(m_damage);

		// TODO projectile settings should include this
		IKnockable knockable = other.GetComponent<IKnockable>();
		knockable?.GetKnocked(transform.position, 0f, 0f);

		m_projectileWeapon.ReleaseProjectileFromPool(this);
	}
}
