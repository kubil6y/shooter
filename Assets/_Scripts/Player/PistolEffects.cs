using System;
using UnityEngine;

public class PistolEffects : MonoBehaviour {
	[SerializeField] private ProjectileWeapon m_pistol;

	private void Start() {
		m_pistol.OnStateChanged += Pistol_OnStateChanged;
	}

    private void Pistol_OnStateChanged(object sender, Weapon.State weaponState) {
        switch (weaponState) {
        case Weapon.State.Idle:
            break;
        case Weapon.State.Fire:
			Debug.Log("Pistol fired!");
            break;
        case Weapon.State.OnCooldown:
            break;
        }
    }
}
