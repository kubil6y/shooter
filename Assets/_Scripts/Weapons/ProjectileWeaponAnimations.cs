using System;
using UnityEngine;

[RequireComponent(typeof(ProjectileWeapon))]
public class ProjectileWeaponAnimations : MonoBehaviour {
	private ProjectileWeapon m_projectileWeapon;
	private Animator m_animator;

	private readonly int ANIMKEY_FIRE = Animator.StringToHash("WeaponFire");

	private void Awake() {
		m_projectileWeapon = GetComponent<ProjectileWeapon>();
		m_animator = GetComponentInChildren<Animator>();
	}

	private void Start() {
		m_projectileWeapon.OnShoot += ProjectileWeapon_OnShoot;
	}

	private void ProjectileWeapon_OnShoot(object sender, EventArgs e) {
		m_animator.Play(ANIMKEY_FIRE, 0, 0f);
	}
}
