using UnityEngine;

public interface IKnockable {
	public void GetKnocked(Vector3 hitDirection, float knockbackThrust, float knockbackDuration);
}
