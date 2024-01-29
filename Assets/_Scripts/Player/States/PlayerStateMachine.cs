using System.Collections.Generic;
using UnityEngine;

public enum PState {
	None,
	Idle,
	Move,
	Dash,
}

public class PlayerStateMachine {
	public PlayerState currentState { get; private set; }
	protected Player player;

	private Dictionary<PState, PlayerState> m_states;

	private PlayerChannel m_playerChannel;
	private bool m_isTransitioning;

	public PlayerStateMachine(Player player) {
		m_states = new Dictionary<PState, PlayerState>();
		this.player = player;

		InitializeStates(player);
	}

	public void ConnectToPlayerChannel() {
		foreach(var state in m_states) {
			state.Value.ConnectToPlayerChannel();
		}
	}

	public void DisconnectFromPlayerChannel() {
		foreach (var state in m_states) {
			state.Value.DisconnectFromPlayerChannel();
		}
	}

	public PState GetCurrentStateKey() {
		return currentState.stateKey;
	}

	public PlayerState GetState(PState stateKey) {
		return m_states[stateKey];
	}

	public void SetState(PState stateKey) {
		PlayerState state = GetState(stateKey);
		if (state == null || m_isTransitioning || stateKey == PState.None) {
			return;
		}
		m_isTransitioning = true;
		currentState?.Exit();
		currentState = GetState(stateKey);
		currentState.Enter();
		m_isTransitioning = false;
	}

	private void AddState(PState stateKey, PlayerState playerState) {
		if (m_states.ContainsKey(stateKey)) {
			Debug.LogError("Duplicated PState key " + stateKey);
			return;
		}
		m_states.Add(stateKey, playerState);
	}

	private void InitializeStates(Player player) {
		PlayerIdleState idleState = new PlayerIdleState(PState.Idle, this, player);
		PlayerMoveState moveState = new PlayerMoveState(PState.Move, this, player);
		PlayerDashState dashState = new PlayerDashState(PState.Dash, this, player);

		AddState(PState.Idle, idleState);
		AddState(PState.Move, moveState);
		AddState(PState.Dash, dashState);
	}
}
