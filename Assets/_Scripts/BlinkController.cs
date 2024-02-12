using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlinkController : MonoBehaviour {
	private SpriteRenderer m_spriteRenderer;

	private bool m_isBlinking;
	private int m_blinkCount = 9;
	private float m_blinkInterval = .1f;
	private Coroutine m_blinkCoroutine;

	private void Awake() {
		m_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public bool IsBlinking() {
		return m_isBlinking;
	}

	public float GetBlinkDuration() {
		return (m_blinkCount + 1) * m_blinkInterval;
	}

	public void StartBlinkingRoutine() {
		m_blinkCoroutine = StartCoroutine(BlinkRoutine());
	}

	public void StopBlinkingRoutine() {
		if (m_blinkCoroutine != null) {
			StopCoroutine(m_blinkCoroutine);
		}
	}

	private IEnumerator BlinkRoutine() {
		m_isBlinking = true;

		for (int i = 0; i < m_blinkCount; i++) {
			m_spriteRenderer.enabled = !m_spriteRenderer.enabled;
			yield return new WaitForSeconds(m_blinkInterval);
		}

		// Ensure the sprite is visible when blinking ends
		m_spriteRenderer.enabled = true;
		m_isBlinking = false;
	}
}
