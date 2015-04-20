using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (HUD))]
public class RTSGame : MonoBehaviour
{

	List<BaseUnit> selectedUnitList = new List<BaseUnit>();
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
		/*
		if (Input.GetMouseButtonDown (0))
		{
			// Navmesh test
			NavMeshAgent[] agents = GameObject.FindObjectsOfType<NavMeshAgent>();
			RaycastHit hitInfo;
			
			Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(screenRay, out hitInfo, 1000.0f, terrainLayer))
			{
				foreach (NavMeshAgent agent in agents)
					agent.SetDestination(hitInfo.point);
			}
		}
		*/

		if (Input.GetMouseButton(1))
		{
			RaycastHit hitInfo;
			
			Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

			bool unitPickedUp = false;
			RaycastHit[] hitInfos = Physics.RaycastAll(screenRay, 1000.0f, unitLayer);
			BaseUnit unit = null;

			foreach (RaycastHit rh in hitInfos)
			{
				//Debug.Log(rh.transform.name);
				BaseUnit v = rh.transform.GetComponentInParent<BaseVehicle>();

				if (v)
				{
					if (unit == null)
					{
						selectedUnitList.Clear();
						unit = v;
					}
					selectedUnitList.Add(v);
					unitPickedUp = true;
				}
			}

			if (!unitPickedUp)
			{
				//RaycastHit hitInfo;

				if (Physics.Raycast(screenRay, out hitInfo, 1000.0f, terrainLayer))
				{
					if (HasSelectedUnits())
					{
						foreach (BaseUnit v in selectedUnitList)
						{
							v.SetMovingDestination(hitInfo.point);
						}
					}
				}
			}
		}
	}

	bool HasSelectedUnits()
	{
		return selectedUnitList.Count != 0;
	}

	public List<BaseUnit> GetSelectedUnits()
	{
		return selectedUnitList;
	}

	public void SetSelectedUnits(List<BaseUnit> list)
	{
		selectedUnitList = list;
	}
}
