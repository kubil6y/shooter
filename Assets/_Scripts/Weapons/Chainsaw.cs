using UnityEngine;

public class Chainsaw : Weapon {
	[SerializeField] private Transform m_attackTf;
	[SerializeField] private float m_attackRadius;

	public override void Perform() {
		Debug.Log("raycast()");
	}
}
