using UnityEngine;

[CreateAssetMenu(fileName = "WeaponPickupDataSO", menuName = "Data/Pickups/AmmoPickupDataSO")]
public class AmmoPickupDataSO : PickupDataSO {
	public WeaponType weaponType;
	public int ammoAmount;
}
