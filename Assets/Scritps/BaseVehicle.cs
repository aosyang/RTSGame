using UnityEngine;
using System.Collections;

public class BaseVehicle : BaseUnit
{
    int terrainLayer = -1;

    public float vehicleSpeed = 5.0f;
	public float vehicleRotateSpeed = 2.0f;

	public bool followSurfaceNormal = true;

    Vector3 targetPoint;
    bool moving = false;
	bool rotating = false;

    Vector3 lastFramePosition;
    
    // Use this for initialization
	public override void Start()
    {
		base.Start();
        terrainLayer = 1 << LayerMask.NameToLayer("Terrain");
		//lastFramePosition = transform.position + transform.InverseTransformVector(new Vector3(0.0f, 0.0f, 1.0f));
		//Debug.Log (lastFramePosition);
		lastFramePosition = transform.position - new Vector3 (0.0f, 0.0f, 1.0f);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
			Vector3 deltaPos = targetPoint - transform.position;

			Vector3 targetDir = Vector3.ProjectOnPlane(deltaPos, transform.up).normalized;

			//Debug.Log (Vector3.Dot(transform.forward, targetDir));

			if (Vector3.Dot(transform.forward, targetDir) >= 0.995f || !rotating)
			{
				rotating = false;

	            deltaPos.y = 0.0f;
	            float planarDist = deltaPos.sqrMagnitude;
	            deltaPos = deltaPos.normalized * Time.deltaTime * vehicleSpeed;
	            
	            if (deltaPos.sqrMagnitude < planarDist)
	                transform.position = transform.position + deltaPos;
	            else
	            {
	                Vector3 targetGroundPos = targetPoint;
	                targetGroundPos.y += GetHeight();
	                transform.position = targetGroundPos;
	                moving = false;
	            }
			}
			
			if (rotating)
			{
				float step = Time.deltaTime * vehicleRotateSpeed;
				Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
				transform.rotation = Quaternion.LookRotation(newDir, transform.up);
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
		if (IsAlive ()) {
			targetPoint = moveTo;
			moving = true;
			rotating = true;
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
