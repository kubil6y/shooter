using System;
using UnityEngine;

public class PistolSounds : MonoBehaviour {
	[SerializeField] private ProjectileWeapon m_pistol;

	private void Start() {
		m_pistol.OnShoot += Pistol_OnShoot;
	}

    private void Pistol_OnShoot(object sender, EventArgs e) {
		AudioManager.instance.PlayPistolFire(transform.position);
    }
}
