using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		if (Input.GetMouseButtonDown (0)) {
			boxSelecting = true;
			selStartPos = Input.mousePosition;
		} else if (Input.GetMouseButtonUp (0)) {
			boxSelecting = false;

			Rect selection = new Rect(selStartPos.x,
			                          Camera.main.pixelRect.height - selStartPos.y,
			                          Input.mousePosition.x - selStartPos.x,
			                          selStartPos.y - Input.mousePosition.y);
			//Debug.Log(selection);
			List<BaseVehicle> vehicleList = new List<BaseVehicle>();

			foreach (BaseVehicle v in GameObject.FindObjectsOfType<BaseVehicle>())
			{
				Vector2 screenPoint = Camera.main.WorldToScreenPoint(v.transform.position);
				screenPoint.y = Camera.main.pixelRect.height - screenPoint.y;
				//Debug.Log (screenPoint);
				if (selection.Contains(screenPoint))
				{
					vehicleList.Add(v);
				}
			}

			game.SetSelectedVehicles(vehicleList);
		}
	}

	void OnGUI()
	{
		foreach (BaseVehicle v in game.GetSelectedVehicles()) {
			Vector3 pos = Camera.main.WorldToScreenPoint(v.transform.position);
			//Debug.Log(Camera.main.pixelRect);
			GUI.Box(new Rect(pos.x - 50, Camera.main.pixelRect.height - pos.y + 20, 100, 20), v.GetTypeName());
		}

		if (boxSelecting) {
			Rect rect = new Rect(selStartPos.x,
			                     Camera.main.pixelRect.height - selStartPos.y,
			                     Input.mousePosition.x - selStartPos.x,
			                     selStartPos.y - Input.mousePosition.y);
			GUI.Box(rect, "");
			//Debug.Log(rect);
		}
	}
}
