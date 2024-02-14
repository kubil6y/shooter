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
	public event EventHandler<OnAmmoPickupEventArgs> OnAmmoPickup;
	public class OnAmmoPickupEventArgs : EventArgs {
		public WeaponType weaponType;
		public int ammoAmount;
	}
	public event EventHandler OnWeaponSwapped;

	[SerializeField] private Transform m_weaponHolderTf;

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

	public bool TrySwappingToNextWeapon() {
		if (!CanSwapWeapon()) {
			return false;
		}
		int nextWeaponIndex = GetNextWeaponIndex();
		if (TrySettingCurrentWeapon(nextWeaponIndex)) {
			m_weaponSwapTimer = m_weaponSwapDuration;
			OnWeaponSwapped?.Invoke(this, EventArgs.Empty);
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
			OnWeaponSwapped?.Invoke(this, EventArgs.Empty);
			return true;
		}
		return false;
	}

	private int GetNextWeaponIndex() {
		for (int i = m_currentWeaponIndex + 1; i < m_weaponArray.Length; i++) {
			if (GetWeapon(i)) {
				return i;
			}
		}
		for (int i = 0; i < m_currentWeaponIndex; i++) {
			if (GetWeapon(i)) {
				return i;
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
		for (int i = m_weaponArray.Length - 1; i > m_currentWeaponIndex; i--) {
			if (GetWeapon(i)) {
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

	private bool TryAddingWeapon(WeaponDataSO weaponDataSO) {
		WeaponType weaponType = weaponDataSO.weaponType;
		int weaponIndex = (int)weaponType;

		if (HasWeapon(weaponType)) {
			return false;
		}

		// Initiate weapon
		Weapon weapon = Instantiate(weaponDataSO.weaponPrefab, m_weaponHolderTf);
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

		// Order of setting weapon to weapon array is important
		// to make it show or hide
		// m_weaponArray[weaponIndex] = weapon;

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

	public Transform GetWeaponHolderTransform() {
		return m_weaponHolderTf;
	}
}
