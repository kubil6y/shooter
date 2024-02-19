using System;
using UnityEngine;

public class RocketLauncherSounds : MonoBehaviour {
	[SerializeField] private ProjectileWeapon m_rocketLauncher;

	private void Start() {
		m_rocketLauncher.OnShoot += RocketLauncher_OnShoot;
	}

    private void RocketLauncher_OnShoot(object sender, EventArgs e) {
		AudioManager.instance.PlayRocketLauncherFire(transform.position);
    }
}
