using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void OnGUI()
	{
		foreach (BaseVehicle v in GameObject.FindObjectsOfType<BaseVehicle>()) {
			Vector3 pos = Camera.main.WorldToScreenPoint(v.transform.position);
			Debug.Log(Camera.main.pixelRect);
			GUI.Box(new Rect(pos.x - 50, Camera.main.pixelRect.height - pos.y + 20, 100, 20), v.GetTypeName());
		}
	}
}
