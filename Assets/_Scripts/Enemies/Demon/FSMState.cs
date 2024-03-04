using System;
using UnityEngine;

[Serializable]
public class FSMState {
	public string id;
	public FSMAction[] actions;
	public FSMTransition[] transitions;

	public void UpdateState(EnemyBrain enemyBrain) {
		ExecuteActions();
		ExecuteTransitions(enemyBrain);
	}

	private void ExecuteActions() {
		foreach (FSMAction action in actions) {
			action.Act();
		}
	}

	private void ExecuteTransitions(EnemyBrain enemyBrain) {
		if (transitions == null || transitions.Length <= 0) return;
		for (int i = 0; i < transitions.Length; i++) {
			bool value = transitions[i].decision.Decide();
			if (value) {
				enemyBrain.ChangeState(transitions[i].trueState);
			}
			else {
				enemyBrain.ChangeState(transitions[i].falseState);
			}
		}
	}
}
