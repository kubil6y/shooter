using System;
using UnityEngine;

public class EvilWizardAnimations : MonoBehaviour {
	public event EventHandler OnAttacked;
	public event EventHandler OnAttackEnded;

	private Animator m_animator;

	private readonly int ANIMKEY_ATTACK = Animator.StringToHash("Attack");

	private void Awake() {
		m_animator = GetComponent<Animator>();
	}

	public void AnimAttackTrigger() {
		OnAttacked?.Invoke(this, EventArgs.Empty);
	}

	public void TriggerAttackAnimation() {
		m_animator.SetTrigger(ANIMKEY_ATTACK);
	}
}
