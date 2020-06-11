using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class BaseVehicle : BaseUnit
{
    int terrainLayer = -1;

    public float vehicleSpeed = 5.0f;
	public float vehicleRotateSpeed = 2.0f;

	public bool followSurfaceNormal = true;

    Vector3 targetPoint;
    bool moving = false;
	bool rotating = false;

    NavMeshPath PathToFollow;
    int NextPathIndex;

    Vector3 lastFramePosition;
    
    // Use this for initialization
	public override void Start()
    {
		base.Start();
        terrainLayer = 1 << LayerMask.NameToLayer("Terrain");
		//lastFramePosition = transform.position + transform.InverseTransformVector(new Vector3(0.0f, 0.0f, 1.0f));
		//Debug.Log (lastFramePosition);
		lastFramePosition = transform.position - new Vector3 (0.0f, 0.0f, 1.0f);

        PathToFollow = new NavMeshPath();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (NextPathIndex < PathToFollow.corners.Length)
            {
                Vector3 MoveTarget = PathToFollow.corners[NextPathIndex];
                Vector3 deltaPos = MoveTarget - transform.position;
                Vector3 targetDir2D = deltaPos;
                targetDir2D.y = 0.0f;
                targetDir2D.Normalize();

                Vector3 forward2D = transform.forward;
                forward2D.y = 0.0f;
                forward2D.Normalize();

                //Debug.Log (Vector3.Dot(transform.forward, targetDir));

                // Check if facing moving direction
                if (Vector3.Dot(forward2D, targetDir2D) >= 0.995f)
                {
                    rotating = false;

                    deltaPos.y = 0.0f;
                    float SqrDistanceToGoal = deltaPos.sqrMagnitude;
                    deltaPos = deltaPos.normalized * Time.deltaTime * vehicleSpeed;

                    // Arrival checking
                    if (deltaPos.sqrMagnitude < SqrDistanceToGoal)
                    {
                        transform.position = transform.position + deltaPos;
                    }
                    else
                    {
                        NextPathIndex++;

                        if (NextPathIndex >= PathToFollow.corners.Length)
                        {
                            // Stop at goal
                            Vector3 targetGroundPos = MoveTarget;
                            targetGroundPos.y += GetHeight();
                            transform.position = targetGroundPos;
                            moving = false;
                        }
                    }
                }
                else
                {
                    rotating = true;
                }

                if (rotating)
                {
                    float step = Time.deltaTime * vehicleRotateSpeed;
                    Vector3 newDir = Vector3.RotateTowards(forward2D, targetDir2D, step, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDir, transform.up);
                }
            }
        }

        // Put vehicle on the ground
        Ray downCastRay = new Ray(transform.position + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down);
        RaycastHit hitInfo;
        if (Physics.Raycast(downCastRay, out hitInfo, 1000.0f, terrainLayer))
        {
            Vector3 groundPos = hitInfo.point;
            groundPos.y += GetHeight();
            transform.position = groundPos;

			//Vector3 planarTarget = groundPos + Vector3.ProjectOnPlane(targetPoint - groundPos, hitInfo.normal);
			Vector3 planarTarget = groundPos - lastFramePosition + groundPos;
			if (followSurfaceNormal)
			{
	            transform.LookAt(planarTarget, hitInfo.normal);
			}
			else
			{
				planarTarget.y = transform.position.y;
				transform.LookAt(planarTarget, Vector3.up);
			}

            lastFramePosition = transform.position;
        }
    }

    float GetHeight()
    {
        return 0.0f;
    }

	public override void SetMovingDestination(Vector3 moveTo)
	{
        if (IsAlive())
        {
            if (NavMesh.CalculatePath(transform.position, moveTo, NavMesh.AllAreas, PathToFollow))
            {
                // Start from the second position
                NextPathIndex = 1;

                targetPoint = moveTo;
                moving = true;
                rotating = true;
            }
            else
            {
                Debug.Log("Path-finding failed!");
            }
        }
    }

    public override Vector3 GetMovingDestination()
	{
		return targetPoint;
	}

	public override void StopMoving()
	{
		moving = false;
		rotating = false;
	}

	public override string GetTypeName()
	{
		return "Tank";
	}
}
