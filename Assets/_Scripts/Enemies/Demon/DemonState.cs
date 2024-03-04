using System;
using UnityEngine;

public class DemonState {
	protected Demon demon;
	public DemonStateMachine stateMachine;

	public DemonState(DemonStateMachine stateMachine, Demon demon) {
		this.stateMachine = stateMachine;
		this.demon = demon;
	}

	public bool IsInThisState() {
		return stateMachine.currentState == this;
	}

	public virtual void ConnectToEvents() {
		Player.instance.health.OnDeath += Player_OnDeath;
	}

    public virtual void DisconnectFromEvents() { }

	public virtual void Enter() { }
	public virtual void Exit() { }
	public virtual void Update() { }
	public virtual void FixedUpdate() { }
	public virtual void OnDrawGizmos() { }

    private void Player_OnDeath(object sender, EventArgs e) {
		stateMachine.ChangeState(stateMachine.playerDeadState);
    }
}
