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
			enemyObject.transform.position = new Vector2(3f, -0.5f);
		}

		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = 0f;
			ObjectPoolManager.instance.SpawnDamagePopup(mousePos, m_damageAmount++);
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			Player.instance.GetKnocked(new Vector3(5, 5), 100, .05f);
			player.TakeDamage(damage1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			// Player.instance.GetKnocked(new Vector3(5, 5), 20f, .2f);
			player.TakeDamage(damage2);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			// Player.instance.GetKnocked(new Vector3(1, 0), 2f, .05f);
			// player.TakeDamage(damage3);
		}

		if (Input.GetKeyDown(KeyCode.Y)) {
			player.TakeDamage(damage2);
		}
		if (Input.GetKeyDown(KeyCode.R)) {
			Player.instance.Revive();
			Debug.Log("Blink duration: " + Player.instance.blink.GetBlinkDuration());
		}
	}
}
