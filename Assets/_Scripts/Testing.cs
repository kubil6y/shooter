using UnityEngine;

public class Testing : MonoBehaviour {
	public GameObject damagePopup;
	public GameObject playerObject;
	public GameObject enemyObject;
	public IDamageable player;

	public int damage1 = 10;
	public int damage2 = 30;
	public int damage3 = 60;

	private int m_damageAmount = 10;

	private void Awake() {
		player = playerObject.GetComponent<IDamageable>();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.T)) {
			playerObject.transform.position = Vector2.zero;
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			Player.instance.Revive();
			Debug.Log("Blink duration: " + Player.instance.blink.GetBlinkDuration());
		}
	}
}
