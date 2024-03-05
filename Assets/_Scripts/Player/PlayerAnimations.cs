using System;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {
	public event EventHandler OnAnimDashStartFinished;
	public event EventHandler OnAnimDashEndFinished;
	public event EventHandler OnAnimUltimateFire;
	public event EventHandler OnAnimUltimateEnded;
	public event EventHandler OnAnimDeathFinished;

	private Player m_player;
	private Animator m_animator;

	private readonly int ANIMKEY_IDLE = Animator.StringToHash("Idle");
	private readonly int ANIMKEY_MOVE = Animator.StringToHash("Move");
	private readonly int ANIMKEY_DEATH = Animator.StringToHash("Death");
	private readonly int ANIMKEY_DASH_START = Animator.StringToHash("DashStart");
	private readonly int ANIMKEY_DASH_END = Animator.StringToHash("DashEnd");
	private readonly int ANIMKEY_ULTIMATE = Animator.StringToHash("Ultimate");

	private void Awake() {
		m_animator = GetComponent<Animator>();
		m_player = GetComponentInParent<Player>();
	}

	public void AnimDeathFinished() {
		OnAnimDeathFinished?.Invoke(this, EventArgs.Empty);
	}

	public void AnimUltimateFireTrigger() {
		OnAnimUltimateFire?.Invoke(this, EventArgs.Empty);
	}

	public void AnimUltimateEndedTrigger() {
		OnAnimUltimateEnded?.Invoke(this, EventArgs.Empty);
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

	public void SetUltimateAnim(bool value) {
		m_animator.SetBool(ANIMKEY_ULTIMATE, value);
	}
	#endregion // Animation Setters
}
