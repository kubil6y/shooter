using System.Collections.Generic;
using UnityEngine;

public class DemonChaseState : DemonState {
	private int m_currentIndex;
	private List<Node> m_pathList;

	private float m_elapsedTime = 0.0f;
	private float m_intervalTime = .5f; //Interval time between path finding

	public DemonChaseState(DemonStateMachine stateMachine, Demon demon) : base(stateMachine, demon) {
	}

	public override void Enter() {
		base.Enter();
		demon.animations.SetChaseAnim(true);
	}

	public override void Exit() {
		base.Exit();
		demon.animations.SetChaseAnim(false);
		m_pathList = null;
	}

	public override void Update() {
		base.Update();

		if (!Player.instance.IsAlive()) {
			stateMachine.ChangeState(stateMachine.playerDeadState);
		}

		float distanceToPlayer = Vector2.Distance(demon.transform.position, Player.instance.transform.position);
		if (demon.CanAttack() && distanceToPlayer < demon.GetAttackDistance()) {
			stateMachine.ChangeState(stateMachine.attackState);
		}

		m_elapsedTime += Time.deltaTime;
		if (m_elapsedTime >= m_intervalTime) {
			m_elapsedTime = 0.0f;
			m_currentIndex = 0;
			FindPath();
		}
		FollowPath();
	}

	private void FindPath() {
		var (startColumn, startRow) = GridManager.instance.GetGridCoordinates(demon.transform.position);
		var (goalColumn, goalRow) = GridManager.instance.GetGridCoordinates(Player.instance.transform.position);
		Node startNode = new Node(GridManager.instance.GetGridCellCenter(startColumn, startRow));
		Node goalNode = new Node(GridManager.instance.GetGridCellCenter(goalColumn, goalRow));
		m_pathList = new AStar().FindPath(startNode, goalNode);
	}

	private void FollowPath() {
		if (m_pathList != null) {
			Vector3 targetPosition = m_pathList[m_currentIndex].position;
			if (Vector2.Distance(demon.transform.position, targetPosition) > 1f) {
				Vector3 moveDir = (targetPosition - demon.transform.position).normalized;
				demon.transform.position += moveDir * demon.GetMoveSpeed() * Time.deltaTime;
			}
			else {
				m_currentIndex++;
				if (m_currentIndex >= m_pathList.Count) {
					m_pathList = null;
				}
			}
		}
	}

	public override void OnDrawGizmos() {
		if (m_pathList == null)
			return;

		if (m_pathList.Count > 0) {
			int index = 1;
			foreach (Node node in m_pathList) {
				if (index < m_pathList.Count) {
					Node nextNode = m_pathList[index];
					Debug.DrawLine(node.position, nextNode.position, Color.green);
					index++;
				}
			};
		}
	}
}
