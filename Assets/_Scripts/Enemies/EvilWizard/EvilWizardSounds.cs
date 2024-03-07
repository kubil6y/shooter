using System;
using UnityEngine;

public class EvilWizardSounds : MonoBehaviour {
	private EvilWizard m_evilWizard;

	private void Awake() {
		m_evilWizard = GetComponent<EvilWizard>();
	}

	private void Start() {
		m_evilWizard.animations.OnAttacked += EvilWizard_OnAttacked;
	}

    private void EvilWizard_OnAttacked(object sender, EventArgs e) {
		AudioManager.instance.PlayEvilWizardAttack(transform.position);
    }
}
