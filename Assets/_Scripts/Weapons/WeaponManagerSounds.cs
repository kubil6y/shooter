using System;
using UnityEngine;

public class WeaponManagerSounds : MonoBehaviour {
	[SerializeField] WeaponManager m_weaponManager;

	private void Start() {
		m_weaponManager.OnAmmoPickup += WeaponManager_OnAmmoPickup;
		m_weaponManager.OnWeaponChanged += WeaponManager_OnWeaponChanged;
	}

    private void WeaponManager_OnAmmoPickup(object sender, WeaponManager.OnAmmoPickupEventArgs e) {
		// TODO ammo pickup
    }

    private void WeaponManager_OnWeaponChanged(object sender, Weapon e) {
		AudioManager.instance.PlayWeaponSwapped(transform.position);
    }

}
