using UnityEngine;
using System.Collections;

public class clouds : MonoBehaviour {

	public float speed = 0.05f;

    //Level bounds
	public float start = -12f;
	public float end = 13f;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(transform.position.x > end) {
			transform.position = new Vector2 (start, transform.position.y);
		}
		else {
			GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.x);
		}
	}
}
