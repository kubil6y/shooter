using UnityEngine;

public class BulletHitVFX : MonoBehaviour {
	public void OnParticleSystemStopped() {
		ObjectPoolManager.instance.RelaseBulletHitVFX(gameObject);
	}
}
