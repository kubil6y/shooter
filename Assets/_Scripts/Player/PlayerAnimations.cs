using System;
using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {
	public event EventHandler OnAnimDashStartFinished;
	public event EventHandler OnAnimDashEndFinished;

	private Player m_player;
	private Animator m_animator;

	private SpriteRenderer m_spriteRenderer;
	private bool m_isBlinking;
	private int blinkCount = 9;
	private float m_blinkInterval = .1f;
	private Coroutine m_blinkCoroutine;

	private readonly int ANIMKEY_IDLE = Animator.StringToHash("Idle");
	private readonly int ANIMKEY_MOVE = Animator.StringToHash("Move");
	private readonly int ANIMKEY_DEATH = Animator.StringToHash("Death");
	private readonly int ANIMKEY_DASH_START = Animator.StringToHash("DashStart");
	private readonly int ANIMKEY_DASH_END = Animator.StringToHash("DashEnd");

	private void Awake() {
		m_animator = GetComponent<Animator>();
		m_player = GetComponentInParent<Player>();
		m_spriteRenderer = GetComponent<SpriteRenderer>();
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

		for (int i = 0; i < blinkCount; i++) {
			m_spriteRenderer.enabled = !m_spriteRenderer.enabled;
			yield return new WaitForSeconds(m_blinkInterval);
		}

		// Ensure the sprite is visible when blinking ends
		m_spriteRenderer.enabled = true;
		m_isBlinking = false;
	}

	public void AnimDashStartFinishedTrigger() {
		OnAnimDashStartFinished?.Invoke(this, EventArgs.Empty);
	}

	public void AnimDashEndFinishedTrigger() {
		OnAnimDashEndFinished?.Invoke(this, EventArgs.Empty);
	}

	#region Animation Setters
	public void SetIdleAnim(bool value) {
		m_animator.SetBool(ANIMKEY_IDLE, value);
	}

	public void SetMoveAnim(bool value) {
		m_animator.SetBool(ANIMKEY_MOVE, value);
	}

	public void SetDeathAnim(bool value) {
		m_animator.SetBool(ANIMKEY_DEATH, value);
	}

	public void SetDashStartAnim(bool value) {
		m_animator.SetBool(ANIMKEY_DASH_START, value);
	}

	public void SetDashEndAnim(bool value) {
		m_animator.SetBool(ANIMKEY_DASH_END, value);
	}
	#endregion // Animation Setters
}
