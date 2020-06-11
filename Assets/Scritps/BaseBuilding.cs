using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseBuilding : BaseBattleObject {

	public Transform unitSpawnPoint;
	public Transform unitRallyPoint;
	public List<GameObject> buildList = new List<GameObject>();

	public float unitReadyTime;
    public int MaxNumSpawned = 10;
    private int CurrentNumSpawned = 0;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        unitReadyTime = Time.time + 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= unitReadyTime)
        {
            if (CurrentNumSpawned < MaxNumSpawned)
            {
                // Select random unit prefab
                int n = Random.Range(0, buildList.Count);

                CurrentNumSpawned++;
                Vector3 spawnPoint = unitSpawnPoint ? unitSpawnPoint.transform.position : transform.position;
                GameObject go = Instantiate(buildList[n], spawnPoint, Quaternion.identity);
                BaseUnit unit = go.GetComponent<BaseUnit>();
                unit.Start();
                unit.SetTeamID(GetTeamID());
                unit.OnUnitDiedEvent += OnSpawnedUnitDied;

                if (unitRallyPoint)
                {
                    unit.SetMovingDestination(unitRallyPoint.position);
                }

                unitReadyTime = Time.time + 2.0f;
            }
        }
    }

    private void OnSpawnedUnitDied()
    {
        CurrentNumSpawned--;
    }
}
