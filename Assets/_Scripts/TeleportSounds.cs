using System;
using UnityEngine;

public class TeleportSounds : MonoBehaviour {
	private Teleport m_teleport;

	private void Awake() {
		m_teleport = GetComponent<Teleport>();
	}

	private void Start() {
		m_teleport.OnTeleIn += Teleport_OnTeleIn;
	}

	private void Teleport_OnTeleIn(object sender, EventArgs e) {
		AudioManager.instance.PlayTeleIn(transform.position);
	}
}
