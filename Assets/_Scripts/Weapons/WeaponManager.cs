using System;
using UnityEngine;

// NOTE: WeaponType manages index positions and size in the WeaponManager

public enum WeaponType {
	Chainsaw,
	Pistol,
	MachineGun,
	LightningGun,
	RailGun,
	Shotgun,
	RocketLauncher,
	__LENGTH,
}

[RequireComponent(typeof(AmmoPouch))]
public class WeaponManager : MonoBehaviour {
	public event EventHandler<Weapon> OnWeaponChanged;
	public event EventHandler OnWeaponPickup; // TODO not handled yet
	public event EventHandler OnAmmoPickup;

	[SerializeField] private Transform m_weaponHolderTf;
	[SerializeField] private WeaponDataSO[] m_startingWeaponSoArray;

	private Weapon[] m_weaponArray;
	private AmmoPouch m_ammoPouch;
	private WeaponManagerVisuals m_weaponManagerVisuals;

	private Player m_player;
	private float m_weaponSwapDuration = .1f;
	private float m_weaponSwapTimer;
	private int m_currentWeaponIndex = -1;

	private void Awake() {
		Init();
		m_player = GetComponentInParent<Player>();
		m_ammoPouch = GetComponent<AmmoPouch>();
		m_weaponManagerVisuals = GetComponent<WeaponManagerVisuals>();
	}

	private void Start() {
		SetStartingWeapons();
	}

	private void Update() {
		HandleWeaponHolderRotation();
		HandleWeaponSwapTimer();
	}

	private void Init() {
		int weaponArrayLength = (int)WeaponType.__LENGTH;
		m_weaponArray = new Weapon[weaponArrayLength];
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
		GetCurrentWeapon()?.Show();
		m_weaponManagerVisuals.ShowHands();
	}

	public void HideVisuals() {
		GetCurrentWeapon()?.Hide();
		m_weaponManagerVisuals.HideHands();
	}

	private void HandleWeaponSwapTimer() {
		m_weaponSwapTimer -= Time.deltaTime;
	}

	public void SetStartingWeapons() {
		foreach (WeaponDataSO weaponDataSO in m_startingWeaponSoArray) {
			TryAddingWeapon(weaponDataSO);
		}
	}

	public bool TrySwappingToNextWeapon() {
		if (!CanSwapWeapon()) {
			return false;
		}
		int nextWeaponIndex = GetNextAvailableWeaponIndex();
		if (TrySettingCurrentWeapon(nextWeaponIndex)) {
			m_weaponSwapTimer = m_weaponSwapDuration;
			OnWeaponChanged?.Invoke(this, GetCurrentWeapon());
			return true;
		}
		return false;
	}

	public bool TrySwappingToPreviousWeapon() {
		if (!CanSwapWeapon()) {
			return false;
		}
		int previousWeaponIndex = GetPreviousAvailableWeaponIndex();
		if (TrySettingCurrentWeapon(previousWeaponIndex)) {
			m_weaponSwapTimer = m_weaponSwapDuration;
			OnWeaponChanged?.Invoke(this, GetCurrentWeapon());
			return true;
		}
		return false;
	}

	// TODO skip out of ammo weapons!
	private int GetNextAvailableWeaponIndex() {
		for (int i = m_currentWeaponIndex + 1; i < m_weaponArray.Length; i++) {
			Weapon weapon = GetWeapon(i);
			if (weapon != null && weapon.IsAvailable()) {
				return i;
			}
		}
		for (int i = 0; i < m_currentWeaponIndex; i++) {
			Weapon weapon = GetWeapon(i);
			if (weapon != null && weapon.IsAvailable()) {
				return i;
			}
		}
		return m_currentWeaponIndex;
	}

	public int GetPreviousAvailableWeaponIndex() {
		for (int i = m_currentWeaponIndex - 1; i >= 0; i--) {
			Weapon weapon = GetWeapon(i);
			if (weapon != null && weapon.IsAvailable()) {
				return i;
			}
		}
		for (int i = m_weaponArray.Length - 1; i > m_currentWeaponIndex; i--) {
			Weapon weapon = GetWeapon(i);
			if (weapon != null && weapon.IsAvailable()) {
				return i;
			}
		}
		return m_currentWeaponIndex;
	}

	private bool CanSwapWeapon() {
		if (!m_player.IsAlive()) {
			return false;
		}
		if (m_weaponSwapTimer > 0f) {
			return false;
		}
		if (GetEquippedWeaponCount() <= 1) {
			return false;
		}
		Weapon currentWeapon = GetCurrentWeapon();
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
		foreach (Weapon weapon in m_weaponArray) {
			if (weapon) {
				weaponCount++;
			}
		}
		return weaponCount;
	}

	private Weapon GetCurrentWeapon() {
		return GetWeapon(m_currentWeaponIndex);
	}

	public bool TrySettingCurrentWeapon(WeaponType weaponType) {
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

		if (GetCurrentWeapon()?.IsOnCooldown() == true) {
			return false;
		}

		GetCurrentWeapon()?.Hide();
		m_currentWeaponIndex = weaponIndex;
		GetCurrentWeapon().Show();
		GetCurrentWeapon().SetAsCurrent();

		OnWeaponChanged?.Invoke(this, GetCurrentWeapon());
		return true;
	}

	private Weapon GetWeapon(int weaponIndex) {
		if (weaponIndex < 0 || weaponIndex >= m_weaponArray.Length) {
			return null;
		}
		return m_weaponArray[weaponIndex];
	}

	private Weapon GetWeapon(WeaponType weaponType) {
		return m_weaponArray[(int)weaponType];
	}

	public bool HasWeapon(WeaponType weaponType) {
		return GetWeapon(weaponType) != null;
	}

	public bool HasAnyWeapon() {
		return m_currentWeaponIndex != -1;
	}

	public void PickUpWeapon(WeaponPickupDataSO weaponPickupDataSO) {
		if (!m_player.CanPickup()) {
			return;
		}

		WeaponType weaponType = weaponPickupDataSO.weaponDataSO.weaponType;

		if (HasWeapon(weaponType)) {
			Weapon weapon = GetWeapon(weaponType);
			if (weapon.TryGetComponent<IHasAmmo>(out IHasAmmo hasAmmo)) {
				int ammoAmount = hasAmmo.GetStartingAmmo();
				hasAmmo.AddAmmo(ammoAmount);
			}
		}
		else {
			TryAddingWeapon(weaponPickupDataSO.weaponDataSO);
		}

		OnWeaponPickup?.Invoke(this, EventArgs.Empty);
	}

	private bool TryAddingWeapon(WeaponDataSO weaponDataSO) {
		WeaponType weaponType = weaponDataSO.weaponType;
		int weaponIndex = (int)weaponType;

		if (HasWeapon(weaponType)) {
			return false;
		}

		// Initiate weapon
		Weapon weapon = Instantiate(weaponDataSO.weaponPrefab, m_weaponHolderTf);
		weapon.SetWeaponUser(m_player);
		m_weaponArray[weaponIndex] = weapon;

		// Setup up object pool parent if exists
		if (weapon.TryGetComponent<IHasObjectPool>(out IHasObjectPool hasObjectPool)) {
			Transform playerProjectileParentTf = ObjectPoolManager.instance.GetPlayerProjectileParentTransform();
			hasObjectPool.SetupObjectPoolParent(playerProjectileParentTf);
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
			GetCurrentWeapon().SetAsCurrent();
		}
		else {
			weapon.Hide();
		}

		return true;
	}

	public void PickUpAmmo(AmmoPickupDataSO ammoPickupDataSO) {
		if (!m_player.CanPickup()) {
			return;
		}

		WeaponType weaponType = ammoPickupDataSO.weaponType;
		int ammoAmount = ammoPickupDataSO.ammoAmount;

		if (HasWeapon(weaponType)) {
			Weapon weapon = GetWeapon(weaponType);
			weapon.GetComponent<IHasAmmo>()?.AddAmmo(ammoAmount);
		}
		else {
			m_ammoPouch.AddAmmo(weaponType, ammoAmount);
		}

		OnAmmoPickup?.Invoke(this, EventArgs.Empty);
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

	public Transform GetWeaponHolderTransform() {
		return m_weaponHolderTf;
	}
}
