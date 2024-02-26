using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour {
	[SerializeField] Light2D m_light2d;
	[SerializeField] private float m_playIntensity = .6f;
	[SerializeField] private float m_quadIntensity = .3f;
	private Coroutine m_lightChangeRoutine;

	private void Start() {
		m_light2d.intensity = m_playIntensity;
		Player.instance.OnQuadStarted += Player_OnQuadStarted;
		Player.instance.OnQuadEnded += Player_OnQuadEnded;
	}

	private IEnumerator ChangeLightRoutine(float from, float to, float duration) {
		float elapsedTime = 0f;
		float currentIntensity = from;
		while (elapsedTime < duration) {
			float t = elapsedTime / duration;
			currentIntensity = Mathf.Lerp(from, to, t);
			m_light2d.intensity = currentIntensity;
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		m_light2d.intensity = to;
	}

	private void Player_OnQuadStarted(object sender, Player.OnQuadStartedEventArgs e) {
		if (m_lightChangeRoutine != null) {
			StopCoroutine(m_lightChangeRoutine);
		}
		m_lightChangeRoutine = StartCoroutine(ChangeLightRoutine(m_playIntensity, m_quadIntensity, .075f));
	}

	private void Player_OnQuadEnded(object sender, EventArgs e) {
		if (m_lightChangeRoutine != null) {
			StopCoroutine(m_lightChangeRoutine);
		}
		m_lightChangeRoutine = StartCoroutine(ChangeLightRoutine(m_quadIntensity, m_playIntensity, .2f));
	}

}
