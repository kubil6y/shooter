using System;
using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour {
	[SerializeField] private Material m_defaultMaterial;
	[SerializeField] private Material m_whiteFlashMaterial;

	private SpriteRenderer[] m_spriteRenderers;
	private Coroutine m_flashCoroutine;

	private float m_flashDuration = .05f;

	private void Awake() {
		m_spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
	}

	public void StartFlash(float duration) {
		if (m_flashCoroutine != null) {
			StopCoroutine(m_flashCoroutine);
		}
		m_flashCoroutine = StartCoroutine(FlashRoutine(duration));
	}

	private IEnumerator FlashRoutine(float duration) {
		foreach (SpriteRenderer sr in m_spriteRenderers) {
			sr.material = m_whiteFlashMaterial;
		}

		yield return new WaitForSeconds(duration);

		foreach (SpriteRenderer sr in m_spriteRenderers) {
			sr.material = m_defaultMaterial;
		}
	}

}
