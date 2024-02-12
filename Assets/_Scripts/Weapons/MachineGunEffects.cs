using System;
using Cinemachine;
using UnityEngine;

public class MachineGunEffects : MonoBehaviour {
	private ProjectileWeapon m_projectileWeapon;
	private CinemachineImpulseSource m_impulseSource;

	private void Awake() {
		m_projectileWeapon = GetComponent<ProjectileWeapon>();
		m_impulseSource = GetComponent<CinemachineImpulseSource>();
	}

	private void Start() {
		m_projectileWeapon.OnShoot += ProjectileWeapon_OnShoot;
	}

    private void ProjectileWeapon_OnShoot(object sender, EventArgs e) {
		m_impulseSource.GenerateImpulse();
    }
}
