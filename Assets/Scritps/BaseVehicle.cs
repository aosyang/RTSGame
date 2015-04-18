using UnityEngine;
using System.Collections;

public class BaseVehicle : MonoBehaviour
{
    int terrainLayer = -1;

    public float vehicleSpeed = 5.0f;

    Vector3 targetPoint;
    bool moving = false;

    Vector3 lastFramePosition;
    
    // Use this for initialization
    void Start()
    {
        terrainLayer = 1 << LayerMask.NameToLayer("Terrain");
        lastFramePosition = transform.position - new Vector3(0.0f, 0.0f, 1.0f);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            Vector3 deltaPos = targetPoint - transform.position;
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
            transform.LookAt(planarTarget, hitInfo.normal);

            lastFramePosition = transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(screenRay, out hitInfo, 1000.0f, terrainLayer))
            {
                targetPoint = hitInfo.point;
                moving = true;
            }
        }

    }

    float GetHeight()
    {
        return 0.0f;
    }
}
