using System;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Knockback))]
[RequireComponent(typeof(Health))]
public class BaseEnemy : MonoBehaviour, IHittable, IDamageable, IKnockable {
	public event EventHandler<OnHitEventArgs> OnHit;
	public class OnHitEventArgs : EventArgs {
		public float hitDuration;
		public WeaponType weaponType;
	}

	public event EventHandler<int> OnTakenDamage;
	public static event EventHandler OnAnyDeath;
	public int id { get; private set; }
	private static int s_id;

	public Health health { get; private set; }
	public Movement movement { get; private set; }
	public Knockback knockback { get; private set; }
	public Flash flash { get; private set; }

	private bool m_canFlip = true;

	protected virtual void Awake() {
		movement = GetComponent<Movement>();
		knockback = GetComponent<Knockback>();
		health = GetComponent<Health>();
		flash = GetComponent<Flash>();
	}

	protected virtual void Start() {
		id = s_id++;
		health.OnDeath += Health_OnDeath;
	}

	public bool CanFlip() {
		return m_canFlip;
	}

	public void SetCanFlip(bool canFlip) {
		m_canFlip = canFlip;
	}

	public void GetKnocked(Vector3 hitDirection, float knockbackThrust, float knockbackDuration) {
		knockback.GetKnocked(Player.instance.transform.position, knockbackThrust, knockbackDuration);
	}

	public void TakeDamage(int damageAmount) {
		if (!health.IsAlive()) {
			return;
		}
		health.TakeDamage(damageAmount);
		OnTakenDamage?.Invoke(this, damageAmount);
	}

	public virtual void TakeHit(WeaponType weaponType, float hitDuration) {
		OnHit?.Invoke(this, new OnHitEventArgs {
			hitDuration = hitDuration,
			weaponType = weaponType,
		});
	}

	protected virtual void Health_OnDeath(object sender, EventArgs e) {
		OnAnyDeath?.Invoke(this, EventArgs.Empty);
		Destroy(gameObject);
	}
}
