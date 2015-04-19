using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RTSGame : MonoBehaviour
{

	List<BaseVehicle> selVehicleList = new List<BaseVehicle>();
	int unitAndTerrainLayer;

	// Use this for initialization
	void Start ()
	{
		unitAndTerrainLayer = 1 << LayerMask.NameToLayer ("Terrain");
		unitAndTerrainLayer |= 1 << LayerMask.NameToLayer ("Unit");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButton(1))
		{

			Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit[] hitInfos = Physics.RaycastAll(screenRay, 1000.0f, unitAndTerrainLayer);
			BaseVehicle vehicle = null;
			Terrain terrain = null;
			Vector3 terrainClickPoint = Vector3.zero;

			foreach (RaycastHit rh in hitInfos)
			{
				//Debug.Log(rh.transform.name);
				BaseVehicle v = rh.transform.GetComponentInParent<BaseVehicle>();
				Terrain t = rh.transform.GetComponent<Terrain>();

				if (v)
				{
					if (vehicle == null)
					{
						selVehicleList.Clear();
						vehicle = v;
					}
					selVehicleList.Add(v);
				}
				else if (t)
				{
					if (terrain == null)
					{
						terrain = t;
						terrainClickPoint = rh.point;
					}
				}
			}

			if (!vehicle && HasSelectedVehicles())
			{
				foreach (BaseVehicle v in selVehicleList)
				{
					v.SetMovingTarget(terrainClickPoint);
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
