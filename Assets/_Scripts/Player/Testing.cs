using UnityEngine;

public class Testing : MonoBehaviour {
	 public GameObject playerObject;
	 public IDamageable player;

	 public int damage1 = 10;
	 public int damage2 = 30;
	 public int damage3 = 60;

	 private void Awake() {
		player = playerObject.GetComponent<IDamageable>();
	 }

	 private void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			player.TakeDamage(damage1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			player.TakeDamage(damage2);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			player.TakeDamage(damage3);
		}
	 }
}
