using System;
using UnityEngine;

public class Demon : BaseEnemy {
	[SerializeField] private int m_attackDamage = 25;
	[SerializeField] private int m_collisionDamage = 10;
	[SerializeField] private float m_moveSpeed = 4f;
	[SerializeField] private float m_attackDistance = 2f;
	[SerializeField] private float m_attackInverval = 1f;
	[SerializeField] private Transform m_attackRefTf;
	[SerializeField] private float m_attackRadius = 2f;
	[SerializeField] private LayerMask m_targetLayerMask;

	public DemonAnimations animations { get; private set; }
	private DemonStateMachine m_stateMachine;
	private float m_lastAttackTime;

	protected override void Awake() {
		base.Awake();
		animations = GetComponentInChildren<DemonAnimations>();
		m_stateMachine = new DemonStateMachine();
	}

	protected override void Start() {
		base.Start();
		m_stateMachine.Initialize(this);
	}

	private void Update() {
		m_stateMachine.currentState?.Update();
	}

	private void FixedUpdate() {
		m_stateMachine.currentState?.FixedUpdate();
	}

	public void SetAttackTimeNow() {
		m_lastAttackTime = Time.time;
	}

	public void Attack() {
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_attackRefTf.position, m_attackRadius, m_targetLayerMask);
		foreach (Collider2D collider in colliders) {
			if (collider.TryGetComponent<Player>(out Player player)) {
				player.TakeDamage(m_attackDamage);
				player.TakeHit(WeaponType.__LENGTH, hitDuration: .1f);
			}
		}
	}

	public bool CanAttack() {
		return Time.time - m_lastAttackTime > m_attackInverval;
	}

	public float GetMoveSpeed() {
		return m_moveSpeed;
	}

	public float GetAttackDistance() {
		return m_attackDistance;
	}

	private void OnDrawGizmos() {
		if (!Application.isPlaying) {
			return;
		}
		m_stateMachine.currentState?.OnDrawGizmos();
		Gizmos.DrawWireSphere(m_attackRefTf.position, m_attackRadius);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<Player>(out Player player)) {
			player.TakeDamage(m_collisionDamage);
			player.TakeHit(WeaponType.__LENGTH, hitDuration: .05f);
		}
	}
}
