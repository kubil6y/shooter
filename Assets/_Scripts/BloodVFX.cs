using UnityEngine;

public class BloodVFX : MonoBehaviour {
	public void OnParticleSystemStopped() {
		ObjectPoolManager.instance.ReleaseBloodVFX(gameObject);
	}
}
