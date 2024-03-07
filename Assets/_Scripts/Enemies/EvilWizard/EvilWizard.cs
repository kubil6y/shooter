using System;
using UnityEngine;

public class EvilWizard : BaseEnemy {
	public static event EventHandler OnAnyEvilWizardDeath; // TODO remove

	[SerializeField] private int m_crystalAmount = 12;
	[SerializeField] private float m_attackInterval = 10f;
	[SerializeField] private Transform m_attackRefTf;

	public EvilWizardAnimations animations;

	private float m_attackTimer;

	protected override void Awake() {
		base.Awake();
		animations = GetComponentInChildren<EvilWizardAnimations>();
		m_attackTimer = m_attackInterval / 2f;
	}

	protected override void Start() {
		base.Start();
		animations.OnAttacked += EvilWizard_OnAttacked;
		health.OnDeath += EvilWizard_OnAnyEvilWizardDeath;
	}

	private void Update() {
		m_attackTimer -= Time.deltaTime;
		if (m_attackTimer < 0f) {
			m_attackTimer = m_attackInterval;
			animations.TriggerAttackAnimation();
		}
	}

	public void Attack() {
		float angleStep = 360f / m_crystalAmount;
		for (int i = 0; i < m_crystalAmount; i++) {
			float angle = i * angleStep;
			Vector3 spawnPosition = m_attackRefTf.position + Quaternion.Euler(0f, 0f, angle) * Vector3.right;
			Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.right;
			ObjectPoolManager.instance.SpawnEvilCrystal(spawnPosition, direction);
		}
	}

	private void EvilWizard_OnAttacked(object sender, EventArgs e) {
		Attack();
	}

	private void EvilWizard_OnAnyEvilWizardDeath(object sender, EventArgs e) {
		OnAnyEvilWizardDeath?.Invoke(this, EventArgs.Empty);
	}
}
