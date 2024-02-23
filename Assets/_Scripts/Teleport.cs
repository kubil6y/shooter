using System;
using UnityEngine;

public class Teleport : MonoBehaviour {
	public event EventHandler OnTeleIn;

	[SerializeField] private Teleport m_pairTeleport;
	[SerializeField] private Transform m_transferPointTf;

	public Vector2 GetTransformPoint() {
		return m_transferPointTf.position;
	}

	public void TakePassenger(GameObject obj) {
		obj.transform.position = m_transferPointTf.position;
		OnTeleIn?.Invoke(this, EventArgs.Empty);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<ICanTeleport>(out ICanTeleport canTeleport)) {
			m_pairTeleport.TakePassenger(other.gameObject);
		}
	}
}

/*
30.5 1.3
-30.5 1.3
.3 14.5 -90
.3 -13.5 -90
*/
