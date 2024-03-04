using System;
using UnityEngine;

public class DemonAttackState : DemonState {
	public DemonAttackState(DemonStateMachine stateMachine, Demon demon) : base(stateMachine, demon) {
	}

	public override void Enter() {
		base.Enter();
		demon.SetAttackTimeNow();
		demon.animations.SetAttackAnim(true);
	}

    public override void ConnectToEvents() {
        base.ConnectToEvents();
		demon.animations.OnAttacked += Demon_OnAttacked;
		demon.animations.OnAttackEnded += Demon_OnAttackEnded;
    }

    public override void Exit() {
		base.Exit();
		demon.animations.SetAttackAnim(false);
	}

    private void Demon_OnAttacked(object sender, EventArgs e) {
		demon.Attack();
    }

    private void Demon_OnAttackEnded(object sender, EventArgs e) {
		if (!IsInThisState()) {
			return;
		}
		stateMachine.ChangeState(stateMachine.chaseState);
    }
}
