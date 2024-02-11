using System;
using UnityEngine;

public class PistolEffects : MonoBehaviour {
	[SerializeField] private ProjectileWeapon m_pistol;

	private void Start() {
		m_pistol.OnShoot += Pistol_OnShoot;
		m_pistol.OnAmmoChanged += Pistol_OnAmmoChanged;
		m_pistol.OnOutOfAmmo += Pistol_OnOutOfAmmo;
	}

    private void Pistol_OnShoot(object sender, EventArgs e) {
		Debug.Log("Pistol_OnShoot()");
    }

    private void Pistol_OnAmmoChanged(object sender, EventArgs e) {
		Debug.Log("Pistol_OnAmmoChanged()");
    }

    private void Pistol_OnOutOfAmmo(object sender, EventArgs e) {
		Debug.Log("Pistol_OnOutOfAmmo()");
    }
}
