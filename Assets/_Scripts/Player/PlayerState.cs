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

	public virtual void Enter() {
		player.InvokeOnStateEnter(stateKey);
		// Debug.Log(GetType().Name + ":Enter()");
	}

	public virtual void Exit() {
		player.InvokeOnStateExit(stateKey);
		// Debug.Log(GetType().Name + ":Exit()");
	}

	public virtual void Update() {
		// TODO remove later
		if (Input.GetKeyDown(KeyCode.U)) {
			((PlayerDashState)stateMachine.GetState(PState.Dash)).Setup(10f);
		}
		if (Input.GetKeyDown(KeyCode.I)) {
			((PlayerDashState)stateMachine.GetState(PState.Dash)).Setup(20f);
		}

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
