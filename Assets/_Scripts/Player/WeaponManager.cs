using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
	[SerializeField] private Transform m_objectPoolsTf;
	[SerializeField] private Transform m_weaponHolderTf;

	private Player m_player;
	private Weapon[] m_weaponArray;

	private int m_currentWeaponIndex;

	private void Awake() {
		InitializeWeaponArray();
		m_player = GetComponentInParent<Player>();
	}

	private void Start() {
		foreach (Transform child in m_weaponHolderTf) {
			if (child.TryGetComponent<ProjectileWeapon>(out ProjectileWeapon projectileWeapon)) {
				projectileWeapon.Setup(m_objectPoolsTf);
			}
		}
	}

	private void Update() {
		// TODO remove
		if (Input.GetKeyDown(KeyCode.B)) {
			m_currentWeaponIndex = 7;
		}
		if (Input.GetKeyDown(KeyCode.N)) {
			Debug.Log(GetNextWeaponIndex());
		}
		if (Input.GetKeyDown(KeyCode.M)) {
			Debug.Log(GetPreviousWeaponIndex());
		}
	}

	private void InitializeWeaponArray() {
		int weaponArrayLength = (int)WeaponType.__LENGTH;
		m_weaponArray = new Weapon[weaponArrayLength];
		for (int i = 0; i < m_weaponArray.Length; i++) {
			m_weaponArray[i] = null;
		}
	}

	public bool TryAddingWeapon(WeaponDataSO weaponDataSO) {
		if (HasWeapon(weaponDataSO.weaponType)) {
			return false;
		}
		// TODO
		int weaponIndex = (int)weaponDataSO.weaponType;
		Weapon weapon = Instantiate(weaponDataSO.weaponPrefab, m_weaponHolderTf);
		if (weapon.TryGetComponent<IHasObjectPool>(out IHasObjectPool hasObjectPool)) {
			hasObjectPool.Setup(m_objectPoolsTf);
		}

		if (IsWeaponArrayEmpty()) {
			m_currentWeaponIndex = weaponIndex;
		}
		else {
			weapon.Hide();
		}
		m_weaponArray[weaponIndex] = weapon;
		return true;
	}

	public Weapon GetWeapon(WeaponType weaponType) {
		return m_weaponArray[(int)weaponType];
	}

	public bool IsWeaponArrayEmpty() {
		foreach (Weapon weapon in m_weaponArray) {
			if (weapon != null) {
				return false;
			}
		}
		return true;
	}

	public int GetNextWeaponIndex() {
		for (int i = m_currentWeaponIndex + 1; i < m_weaponArray.Length; i++) {
			if (m_weaponArray[i] != null) {
				return i;
			}
		}
		return -1;
	}

	public int GetPreviousWeaponIndex() {
		for (int i = m_currentWeaponIndex - 1; i >= 0; i--) {
			if (m_weaponArray[i] != null) {
				return i;
			}
		}
		return -1;
	}


	public bool HasWeapon(WeaponType weapontype) {
		return m_weaponArray[(int)weapontype] != null;
	}

	public void StartShooting() {
		m_weaponArray[m_currentWeaponIndex]?.Fire();
	}

	public void StopShooting() {
		m_weaponArray[m_currentWeaponIndex]?.StopFiring();
	}

	public void PickupWeapon(WeaponPickupDataSO weaponPickupDataSO) {
		if (HasWeapon(weaponPickupDataSO.weaponDataSO.weaponType)) {
			Debug.Log("add as ammo!");
		}
		else {
			TryAddingWeapon(weaponPickupDataSO.weaponDataSO);
		}
	}
}
