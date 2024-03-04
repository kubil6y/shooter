using System;
using UnityEngine;

public class DemonSounds : MonoBehaviour {
	private Demon m_demon;

	private void Awake() {
		m_demon = GetComponent<Demon>();
	}

	private void Start() {
		m_demon.animations.OnAttacked += Demon_OnAttacked;
	}

    private void Demon_OnAttacked(object sender, EventArgs e) {
		AudioManager.instance.PlayDemonAttacked(transform.position);
    }
}
