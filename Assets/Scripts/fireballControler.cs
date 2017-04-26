using UnityEngine;
using System.Collections;

public class fireballControler : MonoBehaviour {

	public float maxSpeed = 10f;
	bool facingRight = true;
	Animator anim;
	bool fireball = false;
	bool fireballCollider = false;
	float charEnd = 0.35f;
	float speed = 0f;
	bool attatched = true;
	public string player = "nero";
	public float start = 0f;

	// Use this for initialization
	void Start () {
		speed = maxSpeed;
	}

	// Update is called once per frame
	void FixedUpdate () {
		float move = Input.GetAxis ("Horizontal");
		if (move > 0 && !facingRight) {
			Flip ();
		} else if (move < 0 && facingRight) {
			Flip ();
		}
		float xPos = transform.localPosition.x;
		//Debug.Log (xPos);
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log ("Launched at " + speed);
			fireball = true;
			transform.gameObject.AddComponent<Rigidbody2D> ();
		}

		if (xPos > charEnd && !fireballCollider) {
			//Debug.Log("Enable. xpos = " + xPos);
			GetComponent<Collider2D> ().enabled = true;
			fireballCollider = true;
			transform.parent = null;
			attatched = false;
		}

		//anim.SetBool ("fireball", fireball);
		if (fireball) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
		} else {
			Destroy (GetComponent<Rigidbody2D> ());
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			//Debug.Log("Disable. xpos = " + xPos);
			GetComponent<Collider2D> ().enabled = false;
			fireballCollider = false;
			//Detatch from player


		}

		//if (transform.position.x > start) {
		//	transform.parent = GameObject.Find(player).transform;
		//	transform.localPosition = new Vector2(0.076f, -0.067f);
		//	fireball = false;
		//	Destroy (GetComponent<Rigidbody2D> ());
		//	attatched = true;
		//}
		//Debug.Log ("Attatched: " + attatched);
	}

	void Flip()
	{
		if (attatched) {
			facingRight = !facingRight;
			if (facingRight) {
				speed = maxSpeed;
			} else {
				speed = -maxSpeed;
			}
			Debug.Log ("Flipped, speed: " + speed);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		transform.parent = GameObject.Find(player).transform;
		transform.localPosition = new Vector2(0.076f, -0.067f);
		fireball = false;
		Destroy (GetComponent<Rigidbody2D> ());
		attatched = true;
	}
}
