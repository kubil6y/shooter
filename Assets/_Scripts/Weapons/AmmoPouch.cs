using UnityEngine;

public class AmmoPouch : MonoBehaviour {
	// Indexed by WeaponType enum
	private int[] m_ammoArray;

	private void Awake() {
		Init();
	}

	private void Init() {
		m_ammoArray = new int[(int)WeaponType.__LENGTH];
		for (int i = 0; i < m_ammoArray.Length; i++) {
			m_ammoArray[i] = 0;
		}
	}

	public void AddAmmo(WeaponType weaponType, int ammoAmount) {
		m_ammoArray[(int)weaponType] += ammoAmount;
	}

	public int GetAmmo(WeaponType weaponType) {
		return m_ammoArray[(int)weaponType];
	}

	public void SetAmmo(WeaponType weaponType, int ammoAmount) {
		m_ammoArray[(int)weaponType] = ammoAmount;
	}

	public int ConsumeAmmo(WeaponType weaponType) {
		int ammoAmount = GetAmmo(weaponType);
		SetAmmo(weaponType, ammoAmount);
		return ammoAmount;
	}
}
