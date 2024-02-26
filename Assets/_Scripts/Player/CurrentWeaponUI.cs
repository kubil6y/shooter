using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentWeaponUI : MonoBehaviour {
	[SerializeField] private WeaponManagerUI m_weaponManagerUI;
	[SerializeField] private Image m_weaponImage;
	[SerializeField] private TextMeshProUGUI m_ammoText;

	private Weapon m_currentWeapon;

	private void Start() {
		Player.instance.weaponManager.OnStartingWeaponsSet += WeaponManager_OnStartingWeaponsSet;
		Player.instance.weaponManager.OnWeaponChanged += WeaponManager_OnWeaponChanged;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.J)) {
			m_weaponImage.sprite = m_weaponManagerUI.GetWeaponSingle(WeaponType.LightningGun).GetWeaponImageSprite();
		}
	}

	private void SetCurrentWeapon(Weapon weapon) {
		m_currentWeapon = weapon;
	}

	private void SetSprite(Sprite sprite) {
		m_weaponImage.sprite = sprite;
	}

	private void WeaponManager_OnStartingWeaponsSet(object sender, WeaponManager.OnStartingWeaponsSetEventArgs e) {
		SetSprite(m_weaponManagerUI.GetWeaponSingle(e.selectedWeapon).GetWeaponImageSprite());
	}

	private void UpdateWeaponSprite() {
		if (!m_currentWeapon){
			return;
		}
		SetSprite(m_weaponManagerUI.GetWeaponSingle(m_currentWeapon.GetWeaponType()).GetWeaponImageSprite());
	}

	private void UpdateAmmoText() {
		if (!m_currentWeapon) {
			return;
		}
		if (m_currentWeapon.HasUnlimitedAmmo()) {
			m_ammoText.text = String.Empty;
		}
		else {
			m_ammoText.text = $"{m_currentWeapon.GetCurrentAmmo().ToString()}/{m_currentWeapon.GetMaxAmmo().ToString()}";
		}

	}

	private void Weapon_OnAmmoChanged(object sender, EventArgs e) {
		Debug.Log("lkajsdf");
		UpdateAmmoText();
	}

	private void WeaponManager_OnWeaponChanged(object sender, Weapon weapon) {
		weapon.OnAmmoChanged -= Weapon_OnAmmoChanged;
		SetCurrentWeapon(weapon);
		weapon.OnAmmoChanged += Weapon_OnAmmoChanged;

		UpdateWeaponSprite();
		UpdateAmmoText();

	}

}
