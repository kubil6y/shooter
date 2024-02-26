using System;
using UnityEngine;

public class WeaponManagerSounds : MonoBehaviour {
	[SerializeField] WeaponManager m_weaponManager;

	private void Start() {
		m_weaponManager.OnAmmoPickup += WeaponManager_OnAmmoPickup;
		m_weaponManager.OnWeaponChanged += WeaponManager_OnWeaponChanged;
		m_weaponManager.OnWeaponPickup += WeaponManager_OnWeaponPickup;
	}

    private void WeaponManager_OnWeaponChanged(object sender, Weapon e) {
		AudioManager.instance.PlayWeaponSwapped(transform.position);
    }

    private void WeaponManager_OnAmmoPickup(object sender, EventArgs e) {
		AudioManager.instance.PlayAmmoPickup(transform.position);
    }

    private void WeaponManager_OnWeaponPickup(object sender, Weapon e) {
		AudioManager.instance.PlayWeaponPickup(transform.position);
    }
}
