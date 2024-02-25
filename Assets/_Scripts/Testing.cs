using UnityEngine;

public class Testing : MonoBehaviour {
	public GameObject enemyObject;

	private void Update() {
		if (Input.GetKeyDown(KeyCode.L)) {
			Player.instance.health.TakeDamage(1000);
		}

		if (Input.GetKeyDown(KeyCode.T)) {
			Player.instance.transform.position = Vector2.zero;
		}

		if (Input.GetKeyDown(KeyCode.O)) {
			Player.instance.Revive();
			Debug.Log("Blink duration: " + Player.instance.blink.GetBlinkDuration());
		}
	}
}
