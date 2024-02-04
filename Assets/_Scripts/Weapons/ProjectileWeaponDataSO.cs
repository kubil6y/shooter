using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileWeaponDataSO", menuName = "Data/ProjectileWeaponDataSO")]
public class ProjectileWeaponDataSO : WeaponDataSO {
	public Projectile projectilePrefab;
	public float projectileSpeed;
	public int poolSize;

	public int dps;
	public int singleFire;
	public int rof;

	public int ammoUsage = 1;
	public int startingAmmo;
	public int maxAmmo;
}
