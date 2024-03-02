using UnityEngine;

[RequireComponent(typeof(Movement))]
public class EnemyFlipController : MonoBehaviour {
	[SerializeField] private bool m_isFacingRight = true;

	private BaseEnemy m_baseEnemy;
	private Movement m_movement;

	private void Awake() {
		m_baseEnemy = GetComponent<BaseEnemy>();
		m_movement = GetComponent<Movement>();
	}

	private void LateUpdate() {
		HandleFlipping();
	}

	public bool IsFacingRight() {
		return m_isFacingRight;
	}

	private void HandleFlipping() {
		if (!m_baseEnemy.CanFlip()) {
			return;
		}

		if (m_movement.GetVelocity().x > 0f && !IsFacingRight()) {
			FlipVisual();
		} else if (m_movement.GetVelocity().x < 0f && IsFacingRight()) {
			FlipVisual();
		}
	}
	private void FlipVisual() {
		var localScale = transform.localScale;
		localScale.x *= -1;
		transform.localScale = localScale;
		m_isFacingRight = !m_isFacingRight;
	}
}
