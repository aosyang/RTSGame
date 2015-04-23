using UnityEngine;
using System.Collections;

public class BaseUnit : BaseBattleObject {
	public float shootingRange = 10.0f;

	// Use this for initialization
	public override void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void DealDamage(int damage)
	{
		if (!IsAlive ())
			return;
		
		base.DealDamage (damage);

		if (life <= 0)
			StopMoving ();
	}

	public bool IsTargetInRange(Vector3 target) { return Vector3.SqrMagnitude (transform.position - target) <= shootingRange * shootingRange; }
	public bool IsTargetInRange(Transform target) { return IsTargetInRange(target.position); }

	public virtual void		SetMovingDestination(Vector3 moveTo)	{}
	public virtual Vector3	GetMovingDestination()					{ return Vector3.zero; }
	public virtual void		StopMoving()							{}
	public virtual string	GetTypeName()							{ return "BaseUnit"; }
}
