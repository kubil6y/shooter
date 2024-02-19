using System;
using UnityEngine;

public class MachineGunSounds : MonoBehaviour {
	[SerializeField] private ProjectileWeapon m_machineGun;

	private void Start() {
		m_machineGun.OnShoot += MachineGun_OnShoot;
	}

    private void MachineGun_OnShoot(object sender, EventArgs e) {
		AudioManager.instance.PlayMGFire(transform.position);
    }
}
