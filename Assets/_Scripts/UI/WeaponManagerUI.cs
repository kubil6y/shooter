using System;
using UnityEngine;

public class WeaponManagerUI : MonoBehaviour {
	[SerializeField] private Transform m_containerTf;
	[SerializeField] private WeaponManagerSingleUI m_chainsaw;
	[SerializeField] private WeaponManagerSingleUI m_pistol;
	[SerializeField] private WeaponManagerSingleUI m_machineGun;
	[SerializeField] private WeaponManagerSingleUI m_lightningGun;
	[SerializeField] private WeaponManagerSingleUI m_railGun;
	[SerializeField] private WeaponManagerSingleUI m_shotgun;
	[SerializeField] private WeaponManagerSingleUI m_rocketLauncher;

	private void Start() {
		HideAll();

		Player.instance.weaponManager.OnWeaponChanged += WeaponManager_OnWeaponChanged;
		Player.instance.weaponManager.OnStartingWeaponsSet += WeaponManager_OnStartingWeaponsSet;
		Player.instance.weaponManager.OnWeaponPickup += WeaponManager_OnWeaponPickup;
	}

    private void SetAsCurrent(WeaponType weaponType) {
		WeaponManagerSingleUI weaponSingle = GetWeaponSingle(weaponType);
		weaponSingle.SetAsSelected();
	}

	private void HideAll() {
		foreach (Transform child in m_containerTf) {
			WeaponManagerSingleUI weaponSingle = child.GetComponent<WeaponManagerSingleUI>();
			weaponSingle.Hide();
		}
	}

	private void SetAllAsUnselected() {
		foreach (Transform child in m_containerTf) {
			WeaponManagerSingleUI weaponSingle = child.GetComponent<WeaponManagerSingleUI>();
			weaponSingle.SetAsUnSelected();
		}
	}

	public WeaponManagerSingleUI GetWeaponSingle(WeaponType weaponType) {
		switch (weaponType) {
		case WeaponType.Chainsaw:
			return m_chainsaw;
		case WeaponType.Pistol:
			return m_pistol;
		case WeaponType.MachineGun:
			return m_machineGun;
		case WeaponType.LightningGun:
			return m_lightningGun;
		case WeaponType.RailGun:
			return m_railGun;
		case WeaponType.Shotgun:
			return m_shotgun;
		case WeaponType.RocketLauncher:
			return m_rocketLauncher;
		default:
			Debug.LogWarning($"WeaponType {weaponType} is not handled!");
			return m_chainsaw;
		}
	}

	private void WeaponManager_OnWeaponChanged(object sender, Weapon weapon) {
		SetAllAsUnselected();
		SetAsCurrent(weapon.GetWeaponType());
	}

	private void WeaponManager_OnStartingWeaponsSet(object sender, WeaponManager.OnStartingWeaponsSetEventArgs e) {
		foreach (WeaponType weaponType in e.weaponArsenal) {
			GetWeaponSingle(weaponType)?.Show();
		}
		SetAsCurrent(e.selectedWeapon);
	}

    private void WeaponManager_OnWeaponPickup(object sender, Weapon weapon) {
		GetWeaponSingle(weapon.GetWeaponType()).Show();
    }
}
