using System;
using UnityEngine;

public class DemonAnimations : MonoBehaviour {
	public event EventHandler OnAttacked;
	public event EventHandler OnAttackEnded;

	private Demon m_demon;
	private Animator m_animator;

	private readonly int ANIMKEY_CHASE = Animator.StringToHash("Chase");
	private readonly int ANIMKEY_ATTACK = Animator.StringToHash("Attack");

	private void Awake() {
		m_animator = GetComponent<Animator>();
		m_demon = GetComponentInParent<Demon>();
	}

	public void AnimAttackTrigger() {
		OnAttacked?.Invoke(this, EventArgs.Empty);
	}

	public void AnimAttackEndedTrigger() {
		OnAttackEnded?.Invoke(this, EventArgs.Empty);
	}

	#region Animation Setters
	public void SetChaseAnim(bool value) {
		m_animator.SetBool(ANIMKEY_CHASE, value);
	}

	public void SetAttackAnim(bool value) {
		m_animator.SetBool(ANIMKEY_ATTACK, value);
	}
	#endregion // Animation Setters
}
