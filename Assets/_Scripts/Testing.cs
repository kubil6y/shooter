using UnityEngine;

public class Testing : MonoBehaviour {
	public Player player;
	public GameObject enemyObject;

	private int m_damageAmount = 10;

	private void Update() {
		if (Input.GetKeyDown(KeyCode.T)) {
			player.transform.position = Vector2.zero;
		}

		if (Input.GetKeyDown(KeyCode.L)) {
			player.Die();
		}

		if (Input.GetKeyDown(KeyCode.O)) {
			Player.instance.Revive();
			Debug.Log("Blink duration: " + Player.instance.blink.GetBlinkDuration());
		}
	}
}
