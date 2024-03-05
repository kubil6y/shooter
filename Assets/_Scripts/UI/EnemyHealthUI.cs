using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour {
	[SerializeField] private BaseEnemy m_enemy;
	[SerializeField] private Image m_healthImage;
	private Coroutine m_updateRoutine;

	private void OnEnable() {
		m_healthImage.fillAmount = 1f; m_enemy.health.OnHealthChanged += Health_OnHealthChanged;
		m_enemy.flip.OnFlipped += Enemy_OnFlipped;
	}

	private void OnDisable() {
		m_enemy.health.OnHealthChanged -= Health_OnHealthChanged;
	}

	private IEnumerator UpdateHealthAmountRoutine() {
		float elapsedTime = 0f;
		float updateDuration = 2f;
		while (elapsedTime < updateDuration) {
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / updateDuration;
			m_healthImage.fillAmount = Mathf.Lerp(m_healthImage.fillAmount, m_enemy.health.GetStartingHealthNormalized(), t);
			yield return null;
		}
		m_healthImage.fillAmount = m_enemy.health.GetStartingHealthNormalized();
	}

	private void FlipVisual() {
		var localScale = transform.localScale;
		localScale.x *= -1;
		transform.localScale = localScale;
	}

	private void Enemy_OnFlipped(object sender, EventArgs e) {
		FlipVisual();
	}

	private void Health_OnHealthChanged(object sender, EventArgs e) {
		if (m_updateRoutine != null) {
			StopCoroutine(m_updateRoutine);
		}
		m_updateRoutine = StartCoroutine(UpdateHealthAmountRoutine());
	}

}
