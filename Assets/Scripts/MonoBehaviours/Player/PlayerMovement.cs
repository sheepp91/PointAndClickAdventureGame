using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {
	public Animator animator;
	public NavMeshAgent agent;
	public float inputHoldDelay = 0.5f;
	public float turnSpeedThreshold = 0.5f;
	public float speedDampTime = 0.1f;
	public float slowingSpeed = 0.175f;

	private WaitForSeconds inputHoldWait;
	private Vector3 destinationPosition;

	private const float stopDistanceProportion = 0.1f;

	private readonly int hashSpeedPara = Animator.StringToHash ("Speed");

	private void Start() {
		agent.updateRotation = false;

		inputHoldWait = new WaitForSeconds (inputHoldDelay);

		destinationPosition = transform.position;
	}

	private void OnAnimatorMove() {
		agent.velocity = animator.deltaPosition / Time.deltaTime;
	}

	private void Update() {
		if (agent.pathPending) { // if the NavMeshAgent is calculating a path
			return;
		}

		float speed = agent.desiredVelocity.magnitude;

		if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion) {
			Stopping (out speed);
		} else if (agent.remainingDistance <= agent.stoppingDistance) {
			Slowing (out speed, agent.remainingDistance);
		} else if (speed > turnSpeedThreshold) {
			Moving ();
		}

		animator.SetFloat (hashSpeedPara, speed, speedDampTime, Time.deltaTime);
	}

	private void Stopping(out float speed) {
		agent.Stop ();
		transform.position = destinationPosition;
		speed = 0f;
	}

	private void Slowing(out float speed, float distanceToDestination) {
		agent.Stop ();
		transform.position = Vector3.MoveTowards (transform.position, destinationPosition, slowingSpeed * Time.deltaTime);
		//https://unity3d.com/learn/tutorials/projects/adventure-game-tutorial/player-continued?playlist=44381
		//Time: 29:30
	}

	private void Moving() {

	}
}
