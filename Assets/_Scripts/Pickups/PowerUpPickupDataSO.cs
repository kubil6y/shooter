using UnityEngine;

public enum PowerUpType {
	Quad,
}

[CreateAssetMenu(fileName = "PowerUpPickupDataSO", menuName = "Data/Pickups/PowerUpPickupDataSO")]
public class PowerUpPickupDataSO : PickupDataSO {
	public PowerUpType powerUpType;
	public float duration;
}
