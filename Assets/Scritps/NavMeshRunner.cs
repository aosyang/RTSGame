using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshRunner : BaseUnit
{
    NavMeshAgent agent;
    float timeUntilReload;
    float timeUntilDeath;
    float timeUntilVanish;
    public GameObject bulletTrail;

    // Duration of death animation
    float DeathAnimDuration = 0.2f;
    float BodyVanishDuration = 5.0f;
    Vector3 DeathPosition;
    BaseUnit lastTarget;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        timeUntilReload = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAlive())
        {
            FireAtEnemyInSight();
        }
        else
        {
            // Update falling animation
            if (timeUntilReload + 1.0f >= Time.time)
            {
                float t = 1.0f - (timeUntilDeath - Time.time) / DeathAnimDuration;

                Vector3 euler = transform.rotation.eulerAngles;
                euler.x = Mathf.Lerp(0, 90, t);
                transform.rotation = Quaternion.Euler(euler);
                transform.position = Vector3.Lerp(DeathPosition, DeathPosition - new Vector3(0.0f, 0.5f, 0.0f), t);
            }

            // Destroy body when time-out
            if (Time.time >= timeUntilVanish)
            {
                Destroy(gameObject);
            }
        }
    }

    void FireAtEnemyInSight()
    {
        if (lastTarget && lastTarget.IsAlive())
        {
            if (IsTargetInRange(lastTarget.transform))
            {
                FireAt(lastTarget);
                return;
            }
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, shootingRange);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            BaseUnit unit = hitColliders[i].gameObject.GetComponent<BaseUnit>();
            if (unit && unit.GetTeamID() != teamID && unit.IsAlive())
            {
                lastTarget = unit;
                FireAt(unit);
                break;
            }
        }
    }

    void FireAt(BaseUnit target)
    {
        if (Time.time > timeUntilReload)
        {
            target.DealDamage(15);

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

    protected override void OnUnitDied()
    {
        base.OnUnitDied();

        agent.enabled = false;
        timeUntilDeath = Time.time + DeathAnimDuration;
        timeUntilVanish = Time.time + BodyVanishDuration;
        DeathPosition = transform.position;
    }

    public override void SetMovingDestination(Vector3 moveTo)
    {
        if (IsAlive())
        {
            agent.SetDestination(moveTo);
        }
    }

    public override Vector3 GetMovingDestination()
    {
        return IsAlive() ? agent.destination : Vector3.zero;
    }

    public override void StopMoving()
    {
        if (IsAlive())
        {
            agent.isStopped = true;
        }
    }

    public override string GetTypeName()
    {
        return "NavMeshAgent";
    }
}
