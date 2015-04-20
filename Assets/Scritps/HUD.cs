using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (RTSGame))]
public class HUD : MonoBehaviour
{
	RTSGame game;
	bool boxSelecting = false;
	Vector3 selStartPos;

	// Use this for initialization
	void Start ()
	{
		game = GetComponent<RTSGame> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			boxSelecting = true;
			selStartPos = Input.mousePosition;
		}
		else if (Input.GetMouseButtonUp (0))
		{
			boxSelecting = false;

			// Make a Rect with selected range, then we project vehicles' positions
			// to the screen and check if they are inside the Rect.

			float x1 = selStartPos.x, y1 = selStartPos.y,
				  x2 = Input.mousePosition.x, y2 = Input.mousePosition.y;
			if (x1 > x2) Swap(ref x1, ref x2);
			if (y1 > y2) Swap(ref y1, ref y2);

			if (x2 - x1 < 4 && y2 - y1 < 4)
			{
				Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				int unitLayer = 1 << LayerMask.NameToLayer ("Unit");

				RaycastHit hitInfo;
				if (Physics.Raycast(screenRay, out hitInfo, 1000.0f, unitLayer))
				{
					List<BaseUnit> unitList = new List<BaseUnit>();
					unitList.Add (hitInfo.transform.GetComponent<BaseUnit>());
					game.SetSelectedUnits(unitList);
				}
			}
			else
			{
				Rect selection = new Rect(x1, y1, x2 - x1, y2 - y1);

				List<BaseUnit> unitList = new List<BaseUnit>();

				foreach (BaseUnit v in GameObject.FindObjectsOfType<BaseUnit>())
				{
					Vector2 screenPoint = Camera.main.WorldToScreenPoint(v.transform.position);

					if (selection.Contains(screenPoint))
					{
						unitList.Add(v);
					}
				}

				game.SetSelectedUnits(unitList);
			}
		}
	}

	void OnGUI()
	{
		foreach (BaseUnit v in game.GetSelectedUnits()) {
			Vector3 pos = Camera.main.WorldToScreenPoint(v.transform.position);
			//Debug.Log(Camera.main.pixelRect);
			GUI.Box(new Rect(pos.x - 50, Camera.main.pixelRect.height - pos.y + 20, 100, 20), v.GetTypeName());
		}

		if (boxSelecting)
		{
			float x1 = selStartPos.x, y1 = selStartPos.y,
				  x2 = Input.mousePosition.x, y2 = Input.mousePosition.y;

			// Swap values in case we have negative width and/or height in Rect
			if (x1 > x2) Swap(ref x1, ref x2);
			if (y1 < y2) Swap(ref y1, ref y2);

			Rect rect = new Rect(x1, Camera.main.pixelRect.height - y1, x2 - x1, y1 - y2);
			GUI.Box(rect, "");
			//Debug.Log(rect);
		}
	}

	public static void Swap<T> (ref T lhs, ref T rhs) {
		T temp = lhs;
		lhs = rhs;
		rhs = temp;
	}

}
