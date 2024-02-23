using System;
using UnityEngine;

public class CrystalAnimations : MonoBehaviour {
	private Crystal m_crystal;
	private Animator m_animator;

	private readonly int ANIMKEY_CRYSTAL_DESTROY_TRIGGER = Animator.StringToHash("CrystalDestroyTrigger");

	private void Awake() {
		m_animator = GetComponent<Animator>();
		m_crystal = GetComponent<Crystal>();
	}

	private void OnEnable() {
		m_crystal.OnCrystalLifetimeEnded += Crystal_OnCrystalLifetimeEnded;
	}

	private void OnDisable() {
		m_crystal.OnCrystalLifetimeEnded -= Crystal_OnCrystalLifetimeEnded;
	}

	public void AnimEndTrigger() {
		Destroy(gameObject);
	}

	private void Crystal_OnCrystalLifetimeEnded(object sender, EventArgs e) {
		m_animator.SetTrigger(ANIMKEY_CRYSTAL_DESTROY_TRIGGER);
	}
}
