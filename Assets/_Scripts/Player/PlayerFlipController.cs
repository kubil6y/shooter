using System;
using UnityEngine;

public class PlayerFlipController : MonoBehaviour {
	private Player m_player;
	private bool m_isFacingRight = true;

	private void Awake() {
		m_player = GetComponentInParent<Player>();
	}

	private void Update() {
		HandleFlipping();
	}

	public bool IsFacingRight() {
		return m_isFacingRight;
	}

	private void HandleFlipping() {
		if (!m_player.CanFlip()) {
			return;
		}

		float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

		if (mouseX > 0f && !m_isFacingRight) {
			FlipPlayerVisual();
		}
		else if (mouseX < 0f && m_isFacingRight) {
			FlipPlayerVisual();
		}
	}

	private void FlipPlayerVisual() {
		var localScale = transform.localScale;
		localScale.x *= -1;
		transform.localScale = localScale;
		m_isFacingRight = !m_isFacingRight;
	}
}
