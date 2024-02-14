using UnityEngine;

public class EnemyDeathVFX : MonoBehaviour {
	private ParticleSystem m_enemyDeathVFX;

	private void Awake() {
		m_enemyDeathVFX = GetComponent<ParticleSystem>();
	}

	public void OnParticleSystemStopped() {
		VFXManager.instance.RelaseEnemyDeathVFX(m_enemyDeathVFX);
	}
}
