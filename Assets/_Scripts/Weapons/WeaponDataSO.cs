using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Data/WeaponDataSO")]
public class WeaponDataSO : ScriptableObject {
	public string weaponName;
	public WeaponType weaponType;

	public Weapon weaponPrefab;
	public Projectile projectilePrefab;
	public int poolSize;

	public int dps; // Damage Per Second
	public int rof; // Rate of Fire
	public int range;

	public bool isSingleFire;
	public int startingAmmo;
	public int maxAmmo;
	public int ammoUsage = 1;
	public int spreadAngle;
	[Range(0, 1f)]
	public float projectileSpeed;

	public bool hasIdleSound;
	public AudioClip idleClip;
	public AudioClip fireFlip;
	public AudioClip quadIdleClip;
	public AudioClip quadFireFlip;
}
