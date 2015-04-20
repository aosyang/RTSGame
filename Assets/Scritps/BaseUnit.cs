using UnityEngine;
using System.Collections;

public class BaseUnit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void		SetMovingDestination(Vector3 moveTo)	{}
	public virtual Vector3	GetMovingDestination()					{ return Vector3.zero; }
	public virtual void		StopMoving()							{}
	public virtual string	GetTypeName()							{ return "BaseUnit"; }
}
