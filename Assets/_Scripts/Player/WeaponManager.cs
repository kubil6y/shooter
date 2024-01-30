using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
	private Player m_player;

	private void Awake() {
		m_player = GetComponentInParent<Player>();
	}
}
