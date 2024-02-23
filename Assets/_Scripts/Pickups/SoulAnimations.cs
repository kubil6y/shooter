using System;
using UnityEngine;

public class SoulAnimations : MonoBehaviour {
	private Soul m_soul;
	private Animator m_animator;

	private readonly int ANIMKEY_SOUL_DESTROY= Animator.StringToHash("SoulDestroy");

	private void Awake() {
		m_animator = GetComponent<Animator>();
		m_soul = GetComponent<Soul>();
	}

	private void OnEnable() {
		m_soul.OnSoulLifetimeEnded += Soul_OnSoulLifetimeEnded;
	}

	private void OnDisable() {
		m_soul.OnSoulLifetimeEnded -= Soul_OnSoulLifetimeEnded;
	}

    private void Soul_OnSoulLifetimeEnded(object sender, EventArgs e) {
		m_animator.Play(ANIMKEY_SOUL_DESTROY, 0, 0f);
    }

    public void SelfDestroy() {
		Destroy(gameObject);
	}
}
