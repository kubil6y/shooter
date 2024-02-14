using UnityEngine;

public class EnemyDeathVFX : MonoBehaviour {
	public void OnParticleSystemStopped() {
		ObjectPoolManager.instance.RelaseEnemyDeathVFX(gameObject);
	}
}
