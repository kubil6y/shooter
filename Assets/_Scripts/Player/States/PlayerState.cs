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
		player.health.OnRevived += Player_OnRevived;
	}

    public virtual void DisconnectFromPlayerChannel() {
		player.health.OnDeath -= Player_OnDeath;
		player.health.OnRevived -= Player_OnRevived;
	}

	public virtual void Enter() {
		// Debug.Log(GetType().Name + ":Enter()");
	}

	public virtual void Exit() {
		// Debug.Log(GetType().Name + ":Exit()");
	}

	public virtual void Update() {
		if (Input.GetKeyDown(KeyCode.T)) {
			player.transform.position = Vector2.zero;
		}
	}

	public virtual void FixedUpdate() { }

	public bool IsInThisState() {
		return stateMachine.GetCurrentStateKey() == stateKey;
	}

    private void Player_OnDeath(object sender, EventArgs e) {
		stateMachine.SetState(PState.Death);
    }

    private void Player_OnRevived(object sender, EventArgs e) {
		stateMachine.SetState(PState.Idle);
    }
}
