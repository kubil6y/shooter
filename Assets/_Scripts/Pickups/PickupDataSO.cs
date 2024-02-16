using UnityEngine;

public class PickupDataSO : ScriptableObject {
	public string pickupName;
	public Sprite icon;
	public float spawnInterval = 10f; // TODO handle default duration
}
