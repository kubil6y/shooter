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
	}

	public virtual void DisconnectFromPlayerChannel() {
	}

	public virtual void Enter() {
		// Debug.Log(GetType().Name + ":Enter()");
	}

	public virtual void Exit() {
		// Debug.Log(GetType().Name + ":Exit()");
	}

	public virtual void Update() {
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			Debug.Log("Shoot! from " + stateKey);
		}
		if (Input.GetKeyDown(KeyCode.T)) {
			player.transform.position = Vector2.zero;
		}
		if (Input.GetKeyDown(KeyCode.I)) {
			stateMachine.SetState(PState.Idle);
		}
		if (Input.GetKeyDown(KeyCode.M)) {
			stateMachine.SetState(PState.Move);
		}
	}

	public virtual void FixedUpdate() { }
}
