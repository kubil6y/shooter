using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour {
	[SerializeField] Light2D m_light2d;
	[SerializeField] private float m_playIntensity = .6f;
	[SerializeField] private float m_countdownIntensity = .2f;

	private void Start() {
		m_light2d.intensity = m_countdownIntensity;
		GameManager.instance.OnCountdownChanged += GameManager_OnCountdownChanged;
	}

	private void GameManager_OnCountdownChanged(object sender, int countdown) {
		if (countdown == 0) {
			m_light2d.intensity = m_playIntensity;
		}
	}
}
