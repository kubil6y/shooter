using UnityEngine;

public class EnemyDeathVFX : MonoBehaviour {
	public void OnParticleSystemStopped() {
		ObjectPoolManager.instance.ReleaseEnemyDeathVFX(gameObject);
	}
}
