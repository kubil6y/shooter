using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

public class DamagePopup : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI m_damageText;

	[SerializeField] private Vector3 m_moveDistance = new Vector3(.2f, 1f, 0f);
	[SerializeField] private Vector3 m_fallDistance = new Vector3(0f, -1f, 0f);

	[SerializeField] private float m_moveDuration = .5f;
	[SerializeField] private float m_fallDuration = .5f;

	private Vector3 m_spawnPosition;


	private void OnEnable() {
		ResetAlpha();
	}

	public void Setup(Vector3 spawnPosition, int damageAmount) {
		transform.position = spawnPosition;
		m_damageText.text = damageAmount.ToString();

		RestartAnimation();
	}

	private void ResetAlpha() {
		Color currentColor = m_damageText.color;
		currentColor.a = 1f;
		m_damageText.color = currentColor;
	}

	private void RestartAnimation() {
		transform.DOMove(transform.position + m_moveDistance, m_moveDuration)
		  .SetEase(Ease.OutQuad)
		  .OnComplete(() => {
			  transform.DOMove(transform.position + m_fallDistance, m_fallDuration)
				  .SetEase(Ease.OutQuad);

			  m_damageText
			  	.DOColor(new Color(m_damageText.color.r, m_damageText.color.g, m_damageText.color.b, 0f), m_fallDuration)
				.OnComplete(() => {
					ObjectPoolManager.instance.ReleaseDamagePopup(gameObject);
				});
		  });
	}
}
