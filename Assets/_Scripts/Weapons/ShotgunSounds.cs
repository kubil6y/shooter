using System;
using UnityEngine;

public class ShotgunSounds : MonoBehaviour {
	[SerializeField] private ProjectileWeapon m_shotgun;

	private void Start() {
		m_shotgun.OnShoot += Shotgun_OnShoot;
	}

    private void Shotgun_OnShoot(object sender, EventArgs e) {
		AudioManager.instance.PlayShotgunGunFire(transform.position);
    }
}
