using System;
using UnityEngine;

[RequireComponent(typeof(KB_AmmoPouch))]
public class KB_WeaponManager : MonoBehaviour {
	public event EventHandler<KB_Weapon> OnWeaponChanged;
	public event EventHandler<OnAmmoPickupEventArgs> OnAmmoPickup;
	public class OnAmmoPickupEventArgs : EventArgs {
		public WeaponType weaponType;
		public int ammoAmount;
	}

	[SerializeField] private Transform m_objectPoolsTf;
	[SerializeField] private Transform m_weaponHolderTf;

	private KB_Weapon[] m_weaponArray;
	private KB_AmmoPouch m_ammoPouch;
	private KB_WeaponManagerVisuals m_weaponManagerVisuals;

	private float m_weaponSwapDuration = .1f;
	private float m_weaponSwapTimer;
	private int m_currentWeaponIndex;

	private void Awake() {
		Init();
		m_ammoPouch = GetComponent<KB_AmmoPouch>();
		m_weaponManagerVisuals = GetComponent<KB_WeaponManagerVisuals>();
	}

	private void Update() {
		HandleWeaponHolderRotation();
		HandleWeaponSwapTimer();

		// TODO TEST
		if (Input.GetKeyDown(KeyCode.L)) {
			Debug.Log(HasAnyWeapon());
		}
	}

	private void Init() {
		int weaponArrayLength = (int)WeaponType.__LENGTH;
		m_weaponArray = new KB_Weapon[weaponArrayLength];
		for (int i = 0; i < weaponArrayLength; i++) {
			m_weaponArray[i] = null;
		}
	}

	public void StartShooting() {
		GetCurrentWeapon()?.StartShooting();
	}

	public void StopShooting() {
		GetCurrentWeapon()?.StopShooting();
	}

	public void ShowVisuals() {
		m_weaponManagerVisuals.ShowVisuals();
	}

	public void HideVisuals() {
		m_weaponManagerVisuals.HideVisuals();
	}

	private void HandleWeaponSwapTimer() {
		m_weaponSwapTimer -= Time.deltaTime;
	}

	public bool TrySwappingToNextWeapon() {
		if (!CanSwapWeapon()) {
			return false;
		}
		int nextWeaponIndex = GetNextWeaponIndex();
		if (TrySettingCurrentWeapon(nextWeaponIndex)) {
			m_weaponSwapTimer = m_weaponSwapDuration;
			return true;
		}
		return false;
	}

	public bool TrySwappingToPreviousWeapon() {
		if (!CanSwapWeapon()) {
			return false;
		}
		int previousWeaponIndex = GetPreviousWeaponIndex();
		if (TrySettingCurrentWeapon(previousWeaponIndex)) {
			m_weaponSwapTimer = m_weaponSwapDuration;
			return true;
		}
		return false;
	}

	private int GetNextWeaponIndex() {
		for (int i = m_currentWeaponIndex; i < m_weaponArray.Length; i++) {
			if (GetWeapon(i)) {
				return i;
			}
		}
		for (int j = 0; j < m_currentWeaponIndex; j++) {
			if (GetWeapon(j)) {
				return j;
			}
		}
		return m_currentWeaponIndex;
	}

	public int GetPreviousWeaponIndex() {
		for (int i = m_currentWeaponIndex - 1; i >= 0; i--) {
			if (GetWeapon(i)) {
				return i;
			}
		}
		for (int j = m_weaponArray.Length - 1; j > m_currentWeaponIndex; j--) {
			if (GetWeapon(j)) {
				return j;
			}
		}
		return m_currentWeaponIndex;
	}

	private bool CanSwapWeapon() {
		if (m_weaponSwapTimer > 0f) {
			return false;
		}
		if (GetEquippedWeaponCount() <= 1) {
			return false;
		}
		KB_Weapon currentWeapon = GetCurrentWeapon();
		if (!currentWeapon) {
			return false;
		}
		if (currentWeapon.IsOnCooldown()) {
			return false;
		}
		return true;
	}

	public int GetEquippedWeaponCount() {
		int weaponCount = 0;
		foreach (KB_Weapon weapon in m_weaponArray) {
			if (weapon) {
				weaponCount++;
			}
		}
		return weaponCount;
	}

	private KB_Weapon GetCurrentWeapon() {
		return GetWeapon(m_currentWeaponIndex);
	}

	private bool TrySettingCurrentWeapon(WeaponType weaponType) {
		return TrySettingCurrentWeapon((int)weaponType);
	}

	private bool TrySettingCurrentWeapon(int weaponIndex) {
		if (weaponIndex < 0 || weaponIndex >= (int)WeaponType.__LENGTH) {
			Debug.LogWarning($"Invalid index: '{weaponIndex}'");
			return false;
		}

		if (m_currentWeaponIndex == weaponIndex) {
			Debug.LogWarning($"m_currentWeaponIndex:'{m_currentWeaponIndex}' == weaponIndex:'{weaponIndex}'");
			return false;
		}

		if (!GetWeapon(weaponIndex)) {
			Debug.LogWarning($"Weapon does not exist! weaponIndex: '{weaponIndex}'");
			return false;
		}

		GetCurrentWeapon()?.Hide();
		m_currentWeaponIndex = weaponIndex;
		GetCurrentWeapon().Show();

		OnWeaponChanged?.Invoke(this, GetCurrentWeapon());
		return true;
	}

	private KB_Weapon GetWeapon(int weaponIndex) {
		return m_weaponArray[weaponIndex];
	}

	private KB_Weapon GetWeapon(WeaponType weaponType) {
		return m_weaponArray[(int)weaponType];
	}

	public bool HasWeapon(WeaponType weaponType) {
		return GetWeapon(weaponType) != null;
	}

	public bool HasAnyWeapon() {
		foreach (KB_Weapon weapon in m_weaponArray) {
			if (weapon != null) {
				return true;
			}
		}
		return false;
	}

	public void PickUpWeapon(WeaponPickupDataSO weaponPickupDataSO) {
		WeaponType weaponType = weaponPickupDataSO.weaponDataSO.weaponType;

		if (HasWeapon(weaponType)) {
			KB_Weapon weapon = GetWeapon(weaponType);
			if (weapon.TryGetComponent<IHasAmmo>(out IHasAmmo hasAmmo)) {
				int ammoAmount = hasAmmo.GetStartingAmmo();
				hasAmmo.AddAmmo(ammoAmount);

				OnAmmoPickup?.Invoke(this, new OnAmmoPickupEventArgs {
					weaponType = weaponType,
					ammoAmount = ammoAmount,
				});
			}
		}
		else {
			TryAddingWeapon(weaponPickupDataSO.weaponDataSO);
		}
	}

	private bool TryAddingWeapon(KB_WeaponDataSO weaponDataSO) {
		WeaponType weaponType = weaponDataSO.weaponType;
		int weaponIndex = (int)weaponType;

		if (HasWeapon(weaponType)) {
			return false;
		}

		// Initiate weapon
		KB_Weapon weapon = Instantiate(weaponDataSO.weaponPrefab, m_weaponHolderTf);

		// Setup up object pool if exists
		if (weapon.TryGetComponent<IHasObjectPool>(out IHasObjectPool hasObjectPool)) {
			hasObjectPool.SetupObjectPoolParent(m_objectPoolsTf);
		}

		// Add ammo from pouch if exists
		if (weapon.TryGetComponent<IHasAmmo>(out IHasAmmo hasAmmo)) {
			hasAmmo.AddStartingAmmo();
			int ammoInPouch = m_ammoPouch.ConsumeAmmo(weaponType);
			hasAmmo.AddAmmo(ammoInPouch);
		}

		// If player has no weapon pick it up and use it
		if (!HasAnyWeapon()) {
			m_currentWeaponIndex = weaponIndex;
			OnWeaponChanged?.Invoke(this, weapon);
		}
		else {
			weapon.Hide();
		}

		// Order of setting weapon to weapon array is important
		// to make it show or hide
		m_weaponArray[weaponIndex] = weapon;

		return true;
	}

	public void PickUpAmmo(AmmoPickupDataSO ammoPickupDataSO) {
		WeaponType weaponType = ammoPickupDataSO.weaponType;
		int ammoAmount = ammoPickupDataSO.ammoAmount;

		if (HasWeapon(weaponType)) {
			KB_Weapon weapon = GetWeapon(weaponType);
			weapon.GetComponent<IHasAmmo>()?.AddAmmo(ammoAmount);
		}
		else {
			m_ammoPouch.AddAmmo(weaponType, ammoAmount);
		}

		OnAmmoPickup?.Invoke(this, new OnAmmoPickupEventArgs {
			weaponType = weaponType,
			ammoAmount = ammoAmount,
		});
	}

	private void HandleWeaponHolderRotation() {
		if (!HasAnyWeapon()) {
			return;
		}
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0f;
		Vector2 weaponToCursorDir = (mousePos - m_weaponHolderTf.position).normalized;
		m_weaponHolderTf.right = weaponToCursorDir;
	}
}
