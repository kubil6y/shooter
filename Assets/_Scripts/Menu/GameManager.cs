using System;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
	[SerializeField] private Transform m_objectPoolContainerTf;

	public Transform GetObjectPoolContainerTransform() {
		return m_objectPoolContainerTf;
	}
}
