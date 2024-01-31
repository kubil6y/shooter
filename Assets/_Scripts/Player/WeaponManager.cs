using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
	[SerializeField] private Transform m_objectPoolsTf;
	[SerializeField] private Transform m_weaponHolderTf;

	private Weapon[] m_weaponArray;
	private Player m_player;
	[SerializeField] private Weapon m_denemeWeapon; // TODO

	private int m_currentWeaponIndex;

	private void Awake() {
		m_player = GetComponentInParent<Player>();
	}

	private void Start() {
		foreach (Transform child in m_weaponHolderTf) {
			if (child.TryGetComponent<ProjectileWeapon>(out ProjectileWeapon projectileWeapon)) {
				projectileWeapon.Setup(m_objectPoolsTf);
			}
		}
	}

	public void StartShooting() {
		// m_weaponArray[m_currentWeaponIndex]?.StartAttack();
		m_denemeWeapon?.Fire();
	}

	public void StopShooting() {
		// m_weaponArray[m_currentWeaponIndex]?.StartAttack();
		m_denemeWeapon?.StopFiring();
	}

	public void PickupWeapon(WeaponPickupDataSO weaponPickupDataSO) {
		Debug.Log("PickupWeapon()");
	}
}
