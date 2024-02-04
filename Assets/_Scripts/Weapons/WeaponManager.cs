using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
	public event EventHandler<Weapon> OnWeaponChanged;
	public event EventHandler<OnAmmoPickupEventArgs> OnAmmoPickup;
	public class OnAmmoPickupEventArgs : EventArgs {
		public WeaponType weaponType;
		public int ammo;
	}

	[SerializeField] private Transform m_objectPoolsTf;
	[SerializeField] private Transform m_weaponHolderTf;

	private Player m_player;
	private Weapon[] m_weaponArray;
	private int[] m_ammoPouch;
	private WeaponManagerVisuals m_weaponManagerVisuals;

	private int m_currentWeaponIndex;
    private float m_swapWeaponTimer;
    private float m_swapBetweenDuration = .1f;

    private void Awake() {
		Init();
		m_player = GetComponentInParent<Player>();
		m_weaponManagerVisuals = GetComponent<WeaponManagerVisuals>();
	}

	private void Update() {
        HandleWeaponSwap();
    }

    private void HandleWeaponSwap() {
        m_swapWeaponTimer -= Time.deltaTime;
        if (GameInput.instance.IsScrollUp() && m_swapWeaponTimer < 0f) {
            m_swapWeaponTimer = m_swapBetweenDuration;
            if (m_weaponArray[m_currentWeaponIndex] != null) {
                if (!m_weaponArray[m_currentWeaponIndex].IsOnCooldown()) {
                    SetCurrentWeapon(GetNextWeaponIndex());
                }
            }
        }
        if (GameInput.instance.IsScrollDown() && m_swapWeaponTimer < 0f) {
            m_swapWeaponTimer = m_swapBetweenDuration;
            if (m_weaponArray[m_currentWeaponIndex] != null) {
                if (!m_weaponArray[m_currentWeaponIndex].IsOnCooldown()) {
                    SetCurrentWeapon(GetPreviousWeaponIndex());
                }
            }
        }
    }

    private void Init() {
		// Initialize weapon array
		int weaponArrayLength = (int)WeaponType.__LENGTH;
		m_weaponArray = new Weapon[weaponArrayLength];
		for (int i = 0; i < m_weaponArray.Length; i++) {
			m_weaponArray[i] = null;
		}
		// Initialize ammo pickups
		m_ammoPouch = new int[weaponArrayLength];
	}

	public void StartShooting() {
		m_weaponArray[m_currentWeaponIndex]?.Fire();
	}

	public void StopShooting() {
		m_weaponArray[m_currentWeaponIndex]?.StopFiring();
	}

	public bool TryAddingWeapon(WeaponDataSO weaponDataSO) {
		if (HasWeapon(weaponDataSO.weaponType)) {
			return false;
		}

		// Initiate weapon
		int weaponIndex = (int)weaponDataSO.weaponType;
		// TODO this changed!
		// Weapon weapon = Instantiate(weaponDataSO.weaponPrefab, m_weaponHolderTf);
		Weapon weapon = null;
		if (weapon.TryGetComponent<IHasObjectPool>(out IHasObjectPool hasObjectPool)) {
			hasObjectPool.SetupObjectPoolParent(m_objectPoolsTf);
		}

		// Add existing ammo to weapon silently
		weapon.AddStartingAmmo();
		weapon.AddAmmo(GetAmmoFromPouch(weaponDataSO.weaponType));
		ClearAmmoPouchForWeapon(weaponDataSO.weaponType);

		// If player has no weapon, pick it up
		if (IsWeaponArrayEmpty()) {
			m_currentWeaponIndex = weaponIndex;
			OnWeaponChanged?.Invoke(this, weapon);
		}
		else {
			weapon.Hide();
		}
		m_weaponArray[weaponIndex] = weapon;

		return true;
	}

	public bool HasWeaponEquipped() {
		return m_weaponArray[m_currentWeaponIndex] != null;
	}

	public void ShowVisuals() {
		m_weaponManagerVisuals.ShowVisuals();
	}

	public void HideVisuals() {
		m_weaponManagerVisuals.HideVisuals();
	}

	public void SetCurrentWeapon(int weaponIndex) {
		if (weaponIndex == m_currentWeaponIndex || m_weaponArray[weaponIndex] == null) {
			return;
		}
		m_weaponArray[m_currentWeaponIndex]?.Hide();
		m_currentWeaponIndex = weaponIndex;
		m_weaponArray[m_currentWeaponIndex].Show();
		OnWeaponChanged?.Invoke(this, m_weaponArray[weaponIndex]);
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
		for (int i = 0; i < m_currentWeaponIndex; i++) {
			if (m_weaponArray[i] != null) {
				return i;
			}
		}
		return m_currentWeaponIndex;
	}

	public int GetPreviousWeaponIndex() {
		for (int i = m_currentWeaponIndex - 1; i >= 0; i--) {
			if (m_weaponArray[i] != null) {
				return i;
			}
		}
		for (int i = m_weaponArray.Length - 1; i > m_currentWeaponIndex; i--) {
			if (m_weaponArray[i] != null) {
				return i;
			}
		}
		return m_currentWeaponIndex;
	}

	public Weapon GetWeapon(WeaponType weaponType) {
		return m_weaponArray[(int)weaponType];
	}

	public bool HasWeapon(WeaponType weapontype) {
		return m_weaponArray[(int)weapontype] != null;
	}

	public void AddAmmoToPouch(WeaponType weaponType, int amount) {
		m_ammoPouch[(int)weaponType] += amount;
	}

	public int GetAmmoFromPouch(WeaponType weaponType) {
		return m_ammoPouch[(int)weaponType];
	}

	public void ClearAmmoPouchForWeapon(WeaponType weaponType) {
		m_ammoPouch[(int)weaponType] = 0;
	}

	public void PickupWeapon(WeaponPickupDataSO weaponPickupDataSO) {
		WeaponType weaponType = weaponPickupDataSO.weaponDataSO.weaponType;

		if (HasWeapon(weaponType)) {
			Weapon weapon = GetWeapon(weaponType);
			int ammoAmount = weapon.GetStartingAmmo();
			weapon.AddAmmo(ammoAmount);

			OnAmmoPickup?.Invoke(this, new OnAmmoPickupEventArgs {
				weaponType = weaponType,
				ammo = GetWeapon(weaponType).GetStartingAmmo(),
			});
		}
		else {
			// TryAddingWeapon(weaponPickupDataSO.weaponDataSO);
		}
	}

	public void PickupAmmo(AmmoPickupDataSO ammoPickupDataSO) {
		if (HasWeapon(ammoPickupDataSO.weaponType)) {
			GetWeapon(ammoPickupDataSO.weaponType).AddAmmo(ammoPickupDataSO.ammoAmount);
		}
		else {
			AddAmmoToPouch(ammoPickupDataSO.weaponType, ammoPickupDataSO.ammoAmount);
		}

		OnAmmoPickup?.Invoke(this, new OnAmmoPickupEventArgs {
			weaponType = ammoPickupDataSO.weaponType,
			ammo = ammoPickupDataSO.ammoAmount,
		});
	}
}
