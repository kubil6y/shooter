using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Data/KB_ProjectileWeaponDataSO")]
public class KB_ProjectileWeaponDataSO : KB_WeaponDataSO {
	public KB_Projectile projectilePrefab;
	public float projectileSpeed;
	public int poolSize;

	public int dps;
	public int singleFire;
	public int rof;

	public int ammoUsage = 1;
	public int startingAmmo;
	public int maxAmmo;
}
