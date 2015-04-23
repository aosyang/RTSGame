using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseBuilding : BaseBattleObject {

	public Transform unitSpawnPoint;
	public List<GameObject> buildList = new List<GameObject>();

	public float unitReadyTime;

	// Use this for initialization
	public override void Start () {
		base.Start ();

		unitReadyTime = Time.time + 2.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= unitReadyTime) {
			int n = Random.Range(0, buildList.Count);

			Vector3 spawnPoint = unitSpawnPoint ? unitSpawnPoint.transform.position : transform.position;
			GameObject go = (GameObject)Instantiate(buildList[n], spawnPoint, Quaternion.identity);
			BaseUnit unit = go.GetComponent<BaseUnit>();
			unit.Start();
			unit.SetTeamID(GetTeamID());
			unit.SetMovingDestination(Vector3.zero);

			unitReadyTime = Time.time + 2.0f;
		}
	}
}
