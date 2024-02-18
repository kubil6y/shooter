using System;
using UnityEngine;

public abstract class PlayerState {
	public PState stateKey { get; private set; }
	public Player player;

	protected PlayerStateMachine stateMachine;

	public PlayerState(PState stateKey, PlayerStateMachine stateMachine, Player player) {
		this.stateMachine = stateMachine;
		this.stateKey = stateKey;
		this.player = player;
	}

	public virtual void ConnectToPlayerChannel() {
		player.health.OnDeath += Player_OnDeath;
	}

	public virtual void DisconnectFromPlayerChannel() {
		player.health.OnDeath -= Player_OnDeath;
	}

	public virtual void Enter() {
	}

	public virtual void Exit() {
	}

	public virtual void Update() { }
	public virtual void FixedUpdate() { }

	public bool IsInThisState() {
		return stateMachine.GetCurrentStateKey() == stateKey;
	}

	private void Player_OnDeath(object sender, EventArgs e) {
		stateMachine.SetState(PState.Death);
	}
}
