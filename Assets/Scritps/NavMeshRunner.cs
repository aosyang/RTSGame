using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NavMeshAgent))]
public class NavMeshRunner : BaseUnit
{
	NavMeshAgent agent;
	float timeUntilReload;
	float timeUntilDead;
	public GameObject bulletTrail;
	float deadTime = 0.2f;
	Vector3 deadPos;

	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		agent = GetComponent<NavMeshAgent> ();
		timeUntilReload = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (IsAlive ()) {
			FireAtEnemyInSight ();
		}
		else if (timeUntilReload + 1.0f >= Time.time)
		{
			float t = 1.0f - (timeUntilDead - Time.time) / deadTime;

			Vector3 euler = transform.rotation.eulerAngles;
			euler.x = Mathf.Lerp(0, 90, t);
			transform.rotation = Quaternion.Euler(euler);
			transform.position = Vector3.Lerp(deadPos, deadPos - new Vector3(0.0f, 0.5f, 0.0f), t);
		}
	}

	void FireAtEnemyInSight()
	{
		Collider[] hitColliders = Physics.OverlapSphere (transform.position, 10.0f);

		for (int i=0; i<hitColliders.Length; i++) {
			BaseUnit unit = hitColliders[i].gameObject.GetComponent<BaseUnit>();
			if (unit && unit.GetTeamID() != teamID && unit.IsAlive())
			{
				FireAt(unit);
				break;
			}
		}
	}

	void FireAt(BaseUnit target)
	{
		if (Time.time > timeUntilReload)
		{
			target.DealDamage (15);

			if (bulletTrail)
			{
				GameObject go = (GameObject)Instantiate(bulletTrail, transform.position, transform.rotation);
				BulletTrail bt = go.GetComponent<BulletTrail>();
				bt.begin = transform.position;
				bt.end = target.transform.position;
			}

			timeUntilReload = Time.time + 0.5f;
		}
	}

	public override void DealDamage(int damage)
	{
		base.DealDamage(damage);
		if (!IsAlive () && agent.enabled == true) {
			agent.enabled = false;
			timeUntilDead = Time.time + deadTime;
			deadPos = transform.position;
		}
	}

	public override void SetMovingDestination(Vector3 moveTo)
	{
		if (IsAlive())
			agent.SetDestination (moveTo);
	}

	public override Vector3 GetMovingDestination()
	{
		return IsAlive() ? agent.destination : Vector3.zero;
	}

	public override void StopMoving()
	{
		if (IsAlive())
			agent.Stop();
	}

	public override string GetTypeName()
	{
		return "NavMeshAgent";
	}
}
