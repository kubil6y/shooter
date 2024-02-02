using UnityEngine;

public class Health : MonoBehaviour {
	// NOTE: Quake3 applies 2/3 of total damage to armor (Some damages: Rail=100, Rocket=100(max), Shotgun=110(max)), then the rest is taken off health.
	[SerializeField] private int m_startingHealth;
	[SerializeField] private int m_startingArmor;
	[SerializeField] private int m_maxHealth;
	[SerializeField] private int m_maxArmor;
}
