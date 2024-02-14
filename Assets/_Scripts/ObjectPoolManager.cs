using System;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager> {
	[SerializeField] private Transform m_playerProjectileParentTf;
	[SerializeField] private Transform m_enemyDeathVFXParentTf;

	public Transform GetPlayerProjectileParentTransform() {
		return m_playerProjectileParentTf;
	}

	public Transform GetEnemyDeathVFXParentTransform() {
		return m_enemyDeathVFXParentTf;
	}
}
