using UnityEngine;

[CreateAssetMenu(fileName = "LightningGunWeaponDataSO", menuName = "Data/LightningGunWeaponDataSO")]
public class LightningGunWeaponDataSO : WeaponDataSO {
	public int damagePerTick;
	public int rof;
	public float range;
	public bool unlimitedAmmo = false;
	public int startingAmmo;
	public int maxAmmo;
	public float knockbackThrust;
	public float knockbackDuration;
}
