using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseBattleObject : MonoBehaviour {
	public int teamID = -1;
	public int life = 300;

	public List<MeshRenderer> teamColorPart = new List<MeshRenderer>();

	// Use this for initialization
	public virtual void Start () {
		SetTeamID(teamID);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void SetTeamID(int id)
	{
		teamID = id;

		RTSGame game = GameObject.FindObjectOfType<RTSGame> ().GetComponent<RTSGame> ();
		
		if (id >= 0 && id < game.teamColorMaterial.Count) {
			for (int i=0; i<teamColorPart.Count; i++) {
				teamColorPart [i].material = game.teamColorMaterial [id];
			}
		}
	}
	public int GetTeamID() { return teamID; }
	
	public virtual void DealDamage(int damage)
	{
		if (!IsAlive ())
			return;
		
		life -= damage;
	}

	
	public bool IsAlive() { return life > 0; }
}
