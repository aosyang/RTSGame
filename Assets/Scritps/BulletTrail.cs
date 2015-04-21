using UnityEngine;
using System.Collections;

public class BulletTrail : MonoBehaviour {
	public Vector3 begin, end;
	public float speed = 1.0f;
	public float delay = 0.5f;
	float time = 0.0f;
	float d = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		d += Time.deltaTime * speed;

		if (d >= 1.0f) {
			delay -= Time.deltaTime;
		}

		transform.position = Vector3.Lerp(begin, end, d > 1.0f ? 1.0f : d);
		if (delay <= 0.0f)
			Destroy (this.gameObject);
	}
}
