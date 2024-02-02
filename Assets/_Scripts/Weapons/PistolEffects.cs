using System;
using UnityEngine;

public class PistolEffects : MonoBehaviour {
	[SerializeField] private ProjectileWeapon m_pistol;

    private readonly int ANIMKEY_FIRE = Animator.StringToHash("Fire");

	private void Start() {
		m_pistol.OnFired += Pistol_OnFired;
		m_pistol.OnOutOfAmmo += Pistol_OnOutOfAmmo;
		m_pistol.OnAmmoChanged += Pistol_OnAmmoChanged;
	}

    private void Pistol_OnFired(object sender, EventArgs e) {
        // Debug.Log("Pistol_OnFired()");
    }

    private void Pistol_OnOutOfAmmo(object sender, EventArgs e) {
        // Debug.Log("Pistol_OnOutOfAmmo()");
    }


    private void Pistol_OnAmmoChanged(object sender, EventArgs e) {
        // Debug.Log("Pistol_OnAmmoChanged()");
    }
}
