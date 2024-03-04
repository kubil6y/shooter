using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TestCode : MonoBehaviour {
	public float moveSpeed = 10f;
	public int currentIndex;

	private Transform startPos, endPos;
	public Node startNode { get; set; }
	public Node goalNode { get; set; }

	public List<Node> pathArray;

	public GameObject objStartCube, objEndCube;

	private float elapsedTime = 0.0f;
	public float intervalTime = 1.0f; //Interval time between path finding

	// Use this for initialization
	void Start() {
		//AStar Calculated Path
		pathArray = new List<Node>();
		FindPath();
	}

	// Update is called once per frame
	void Update() {
		elapsedTime += Time.deltaTime;

		if (elapsedTime >= intervalTime) {
			elapsedTime = 0.0f;
			currentIndex = 0;
			FindPath();
		}
		FollowPath();
	}

	private void FollowPath() {
		if (pathArray != null) {
			Vector3 targetPosition = pathArray[currentIndex].position;
			if (Vector2.Distance(objStartCube.transform.position, targetPosition) > 1f) {
				Vector3 moveDir = (targetPosition - objStartCube.transform.position).normalized;

				objStartCube.transform.position += moveDir * moveSpeed * Time.deltaTime;
			}
			else {
				currentIndex++;
				if (currentIndex >= pathArray.Count) {
					pathArray = null;
				}
			}
		}
	}

	void FindPath() {
		startPos = objStartCube.transform;
		endPos = objEndCube.transform;

		//Assign StartNode and Goal Node
		var (startColumn, startRow) = GridManager.instance.GetGridCoordinates(startPos.position);
		var (goalColumn, goalRow) = GridManager.instance.GetGridCoordinates(endPos.position);
		startNode = new Node(GridManager.instance.GetGridCellCenter(startColumn, startRow));
		goalNode = new Node(GridManager.instance.GetGridCellCenter(goalColumn, goalRow));

		pathArray = new AStar().FindPath(startNode, goalNode);
	}

	void OnDrawGizmos() {
		if (pathArray == null)
			return;

		if (pathArray.Count > 0) {
			int index = 1;
			foreach (Node node in pathArray) {
				if (index < pathArray.Count) {
					Node nextNode = pathArray[index];
					Debug.DrawLine(node.position, nextNode.position, Color.green);
					index++;
				}
			};
		}
	}
}
