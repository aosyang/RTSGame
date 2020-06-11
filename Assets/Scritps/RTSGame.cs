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

	public List<Material> teamColorMaterial = new List<Material>();

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
        // Right click to move units
		if (Input.GetMouseButtonDown(1))
		{
			RaycastHit hitInfo;
			
			Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

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
