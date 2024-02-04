using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Data/WeaponDataSO")]
public class WeaponDataSO : ScriptableObject {
	public string weaponName;
	public WeaponType weaponType;
	public Weapon weaponPrefab;
}
