using System;
using UnityEngine;

public class ProjectileWeaponSounds : MonoBehaviour {
	[SerializeField] private ProjectileWeapon m_projectileWeapon;

	private void Start() {
		m_projectileWeapon.OnOutOfAmmo += ProjectileWeapon_OnOutOfAmmo;
	}

    private void ProjectileWeapon_OnOutOfAmmo(object sender, EventArgs e) {
		AudioManager.instance.PlayOutOfAmmo(transform.position);
    }
}
