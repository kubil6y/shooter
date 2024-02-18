using System;
using UnityEngine;

public class ChainsawEffects : MonoBehaviour {
	[SerializeField] private Chainsaw m_chainsaw;

	private void OnEnable() {
		m_chainsaw.OnCut += Chainsaw_OnCut;
	}

	private void OnDisable() {
		m_chainsaw.OnCut -= Chainsaw_OnCut;
	}

	private void Chainsaw_OnCut(object sender, Vector2 position) {
		ObjectPoolManager.instance.SpawnBloodVFX(position);
	}
}
