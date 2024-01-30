using System;
using UnityEngine;

public class PlayerChannel : MonoBehaviour {
	public event EventHandler OnAnimDashStartFinished;
	public event EventHandler OnAnimDashEndFinished;
	public event EventHandler<OnPlayerFlippedEventArgs> OnPlayerFlipped;
	public class OnPlayerFlippedEventArgs: EventArgs {
		public bool isFacingRight;
	}

	private Player m_player;

	private void Awake() {
		m_player = GetComponent<Player>();
	}

	public void Emit_OnAnimDashStartFinished(object sender) {
		OnAnimDashStartFinished?.Invoke(sender, EventArgs.Empty);
	}

	public void Emit_OnAnimDashEndFinished (object sender) {
		OnAnimDashEndFinished?.Invoke(this, EventArgs.Empty);
	}

	public void Emit_OnPlayerFlipped(object sender, bool isFacingRight) {
		OnPlayerFlipped?.Invoke(sender, new OnPlayerFlippedEventArgs {
			isFacingRight = isFacingRight,
		});
	}
}
