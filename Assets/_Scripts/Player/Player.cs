using System;
using UnityEngine;

public class Player : MonoBehaviour {
	public event EventHandler<PState> OnStateEnter;
	public event EventHandler<PState> OnStateExit;

	[Header("Dash Skill")]
	[field: SerializeField] public float dashSpeed { get; private set; } = 5f;
	[field: SerializeField] public float dashDuration { get; private set; } = 1f;

	public Rigidbody2D rb { get; private set; }

	private PlayerStateMachine m_stateMachine;

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start() {
		m_stateMachine = new PlayerStateMachine(this);
		m_stateMachine.SetState(PState.Idle);
	}

	private void Update() {
		if (GameInput.instance.Dash_WasPerformedThisFrame()) {
			m_stateMachine.SetState(PState.Dash);
		}

		m_stateMachine.currentState?.Update();

		// Debug.Log(m_stateMachine.currentState.stateKey); // TODO
	}

	private void FixedUpdate() {
		m_stateMachine.currentState?.FixedUpdate();
	}

	// TODO test these
	public void InvokeOnStateEnter(PState stateKey) {
		OnStateEnter?.Invoke(this, stateKey);
	}

	// TODO test these
	public void InvokeOnStateExit(PState stateKey) {
		OnStateExit?.Invoke(this, stateKey);
	}
}
