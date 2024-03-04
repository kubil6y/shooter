using UnityEngine;

public class EnemyBrain : MonoBehaviour {
	[SerializeField] private string m_initState; // PatrolState
	[SerializeField] private FSMState[] m_states;

	public FSMState CurrentState { get; set; }
	public Transform Player { get; set; }

	private void Start() {
		ChangeState(m_initState);
	}

	private void Update() {
		CurrentState?.UpdateState(this);
	}

	public void ChangeState(string newStateID) {
		FSMState newState = GetState(newStateID);
		if (newState == null) return;
		CurrentState = newState;
	}

	private FSMState GetState(string newStateID) {
		for (int i = 0; i < m_states.Length; i++) {
			if (m_states[i].id == newStateID) {
				return m_states[i];
			}
		}
		return null;
	}
}
