using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	private static T m_instance;
	public static T instance {
		get {
			if (m_instance == null) {
				m_instance = FindFirstObjectByType<T>();
				if (m_instance == null) {
					m_instance = new GameObject("Intance of " + typeof(T)).AddComponent<T>();
				}
			}
			return m_instance;
		}
	}

	protected virtual void Awake() {
		if (m_instance != null) {
			Destroy(gameObject);
		}
	}
}
