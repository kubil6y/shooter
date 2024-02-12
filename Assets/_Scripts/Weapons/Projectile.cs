using UnityEngine;

public class Projectile : MonoBehaviour {
	private float m_moveSpeed = 10f;
	private Rigidbody2D m_rb;
	private ProjectileWeapon m_projectileWeapon;
	private Vector2 m_fireDirection;
	private int m_damage;
	private float m_lifetimer;
	public float m_knockbackThrust;
	public float m_knockbackDuration;
	private LayerMask m_targetLayerMask;

	// TODO: requires knockback settings...
	public class ProjectileSetupArgs {
		public ProjectileWeapon projectileWeapon;
		public Transform spawnTf;
		public Vector2 fireDirection;
		public float moveSpeed;
		public int damage;
		public float lifetime;
		public LayerMask targetLayerMask;
		public float knockbackThrust;
		public float knockbackDuration;
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
		m_targetLayerMask = args.targetLayerMask;
		m_knockbackThrust = args.knockbackThrust;
		m_knockbackDuration = args.knockbackDuration;
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
		// if (((1 << other.gameObject.layer) & m_targetLayerMask) == 0) {
		// 	return;
		// }

		IHittable hittable = other.GetComponent<IHittable>();
		hittable?.TakeHit();

		IDamageable damageable = other.GetComponent<IDamageable>();
		damageable?.TakeDamage(m_damage);

		// TODO projectile settings should include this
		IKnockable knockable = other.GetComponent<IKnockable>();
		knockable?.GetKnocked(transform.position, m_knockbackThrust, m_knockbackDuration);

		m_projectileWeapon.ReleaseProjectileFromPool(this);
	}
}
