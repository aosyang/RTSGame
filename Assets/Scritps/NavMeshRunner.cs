using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NavMeshAgent))]
public class NavMeshRunner : BaseUnit
{
	NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void SetMovingDestination(Vector3 moveTo)
	{
		agent.SetDestination (moveTo);
	}

	public override Vector3 GetMovingDestination()
	{
		return agent.destination;
	}

	public override void StopMoving()
	{
		agent.Stop ();
	}

	public override string GetTypeName()
	{
		return "NavMeshAgent";
	}
}
