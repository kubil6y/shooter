using UnityEngine;

public class EnemyFlipController : MonoBehaviour {
	[SerializeField] private bool m_isFacingRight = true;

	private BaseEnemy m_baseEnemy;

	private void Awake() {
		m_baseEnemy = GetComponent<BaseEnemy>();
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
		if (!Player.instance.transform) {
			return;
		}
		bool playerOnRight = Player.instance.transform.position.x > m_baseEnemy.transform.position.x;

		if (playerOnRight && !IsFacingRight()) {
			FlipVisual();
		} else if (!playerOnRight && IsFacingRight()) {
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
