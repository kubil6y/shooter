using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileWeaponDataSO", menuName = "Data/ProjectileWeaponDataSO")]
public class ProjectileWeaponDataSO : WeaponDataSO {
	public Projectile projectilePrefab;
	public float projectileSpeed;
	public int projectileDamage;
	public float knockbackThrust;
	public float knockbackDuration;
	public int poolSize;
	public int rof;
	public bool singleFire;
	public bool unlimitedAmmo = false;
	[Range(0f, 30f)]
	public float spreadAngle;
	public int ammoUsage = 1;
	public int startingAmmo;
	public int maxAmmo;
}
