using System;
using UnityEngine;

public class PlayerChannel : MonoBehaviour {
	public event EventHandler OnAnimDashStartFinished;
	public event EventHandler OnAnimDashEndFinished;

	private Player m_player;

	private void Awake() {
		m_player = GetComponent<Player>();
	}

	public void Emit_OnAnimDashStartFinished() {
		OnAnimDashStartFinished?.Invoke(this, EventArgs.Empty);
	}

	public void Emit_OnAnimDashEndFinished () {
		OnAnimDashEndFinished?.Invoke(this, EventArgs.Empty);
	}
}
