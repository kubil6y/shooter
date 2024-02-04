using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Data/KB_WeaponDataSO")]
public class KB_WeaponDataSO : ScriptableObject {
	public string weaponName;
	public WeaponType weaponType;
	public KB_Weapon weaponPrefab;
}
