using UnityEngine;

[CreateAssetMenu(fileName = "LightningGunWeaponDataSO", menuName = "Data/LightningGunWeaponDataSO")]
public class LightningGunWeaponDataSO : WeaponDataSO {
	public int damagePerTick;
	public float knockbackThrust;
	public float knockbackDuration;
	public int rof;
	public bool unlimitedAmmo = false;
	public int startingAmmo;
	public int maxAmmo;
}
