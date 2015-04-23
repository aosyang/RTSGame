using UnityEngine;
using System.Collections;

public class BaseUnit : MonoBehaviour {
	public int teamID = -1;
	public int unitLife = 300;
	public float shootingRange = 10.0f;

	// Use this for initialization
	public virtual void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int GetTeamID() { return teamID; }

	public virtual void DealDamage(int damage)
	{
		if (!IsAlive ())
			return;

		unitLife -= damage;
		if (unitLife <= 0)
			StopMoving ();
	}

	public bool IsTargetInRange(Vector3 target) { return Vector3.SqrMagnitude (transform.position - target) <= shootingRange * shootingRange; }
	public bool IsTargetInRange(Transform target) { return IsTargetInRange(target.position); }

	public bool IsAlive() { return unitLife > 0; }

	public virtual void		SetMovingDestination(Vector3 moveTo)	{}
	public virtual Vector3	GetMovingDestination()					{ return Vector3.zero; }
	public virtual void		StopMoving()							{}
	public virtual string	GetTypeName()							{ return "BaseUnit"; }
}
