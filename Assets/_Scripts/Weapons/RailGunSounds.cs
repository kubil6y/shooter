using System;
using UnityEngine;

public class RailGunSounds : MonoBehaviour {
	[SerializeField] private ProjectileWeapon m_railGun;

	private void Start() {
		m_railGun.OnShoot += RailGun_OnShoot;
	}

    private void RailGun_OnShoot(object sender, EventArgs e) {
		AudioManager.instance.PlayRailGunFire(transform.position);
    }
}
