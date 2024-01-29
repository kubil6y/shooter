using System;
using UnityEngine;

// TODO player sprite flip controller should be seperated!

public class PlayerAnimations : MonoBehaviour {
	private Player m_player;
	private Animator m_animator;

	private bool m_facingRight = true;

	private readonly int ANIMKEY_IDLE = Animator.StringToHash("Idle");
	private readonly int ANIMKEY_MOVE = Animator.StringToHash("Move");
	private readonly int ANIMKEY_DEATH = Animator.StringToHash("Death");
	private readonly int ANIMKEY_DASH_START = Animator.StringToHash("DashStart");
	private readonly int ANIMKEY_DASH_END = Animator.StringToHash("DashEnd");

	private void Awake() {
		m_animator = GetComponent<Animator>();
		m_player = GetComponentInParent<Player>();
	}

	private void Update() {
		HandleFlipping();
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

	public void AnimDashStartFinishedTrigger() {
		m_player.channel.Emit_OnAnimDashStartFinished();
	}

	public void AnimDashEndFinishedTrigger() {
		m_player.channel.Emit_OnAnimDashEndFinished();
	}

	private void HandleFlipping() {
		float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

		if (mouseX > 0f && !m_facingRight) {
			FlipPlayerVisual();
		}
		else if (mouseX < 0f && m_facingRight) {
			FlipPlayerVisual();
		}
	}

	private void FlipPlayerVisual() {
		var localScale = transform.localScale;
		localScale.x *= -1;
		transform.localScale = localScale;
		m_facingRight = !m_facingRight;
	}
}
