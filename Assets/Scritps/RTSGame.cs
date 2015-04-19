using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RTSGame : MonoBehaviour
{

	List<BaseVehicle> selVehicleList = new List<BaseVehicle>();
	int unitAndTerrainLayer;
	int unitLayer;
	int terrainLayer;

	// Use this for initialization
	void Start ()
	{
		unitAndTerrainLayer = 1 << LayerMask.NameToLayer ("Terrain");
		unitAndTerrainLayer |= 1 << LayerMask.NameToLayer ("Unit");

		unitLayer = 1 << LayerMask.NameToLayer ("Unit");
		terrainLayer = 1 << LayerMask.NameToLayer ("Terrain");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButton(1))
		{
			bool vehiclePickedUp = false;
			Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit[] hitInfos = Physics.RaycastAll(screenRay, 1000.0f, unitLayer);
			BaseVehicle vehicle = null;

			foreach (RaycastHit rh in hitInfos)
			{
				//Debug.Log(rh.transform.name);
				BaseVehicle v = rh.transform.GetComponentInParent<BaseVehicle>();

				if (v)
				{
					if (vehicle == null)
					{
						selVehicleList.Clear();
						vehicle = v;
					}
					selVehicleList.Add(v);
					vehiclePickedUp = true;
				}
			}

			if (!vehiclePickedUp)
			{
				RaycastHit hitInfo;

				if (Physics.Raycast(screenRay, out hitInfo, 1000.0f, terrainLayer))
				{
					if (HasSelectedVehicles())
					{
						foreach (BaseVehicle v in selVehicleList)
						{
							v.SetMovingTarget(hitInfo.point);
						}
					}
				}
			}
		}
	}

	bool HasSelectedVehicles()
	{
		return selVehicleList.Count != 0;
	}

	public List<BaseVehicle> GetSelectedVehicles()
	{
		return selVehicleList;
	}

	public void SetSelectedVehicles(List<BaseVehicle> list)
	{
		selVehicleList = list;
	}
}
