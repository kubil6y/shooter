using UnityEngine;
// PlayerNormalState is a super state for IdleState and MoveState
public class PlayerNormalState : PlayerState {
	public PlayerNormalState(PState stateKey, PlayerStateMachine stateMachine, Player player) : base(stateKey, stateMachine, player) {
	}
}
