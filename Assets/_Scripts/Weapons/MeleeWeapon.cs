using UnityEngine;

public class MeleeWeapon : Weapon {
	[SerializeField] private Transform m_attackTf;
	[SerializeField] private float m_attackRadius;

	public override void Perform() {
		// TODO raycast
	}
}
