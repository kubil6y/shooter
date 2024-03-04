using System;

[Serializable]
public class FSMTransition {
	public FSMDecision decision; // PlayerInRangeOfAttack -> True or False
	public string trueState; // CurrentState -> AttackState
	public string falseState; // CurrentState -> PatrolState
}
