using UnityEngine;
using System.Collections;

public class fireballControler : MonoBehaviour {

	public float maxSpeed = 10f;
	float speed = 0f;
	bool fireball = false;
	bool facingRight = true;
	string player = "nero";
	float charEnd = 1.8f;
	bool fireballCollider = false;
	public float start = 0f;
	public float end = 0f;
	bool fireballAnim = false;

	// Use this for initialization
	void Start () {
		speed = maxSpeed;
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector2 playerLoc = GameObject.Find (player).transform.position;
		float x = playerLoc.x + 0.581f;
		float y = playerLoc.y - 0.2882f;
		GetComponent<Animator> ().SetBool ("fireball", fireball);

		if (Input.GetKeyDown (KeyCode.Space) && !fireball) {
			transform.position = new Vector2 (x, y);
			transform.gameObject.AddComponent<Rigidbody2D> ();
			GetComponent<Collider2D> ().enabled = false;
			fireballCollider = false;
			fireball = true;
			GameObject.Find (player).GetComponent<Animator> ().SetBool ("fireball", true);
			fireballAnim = true;
			Debug.Log ("Enable");

		}

		if (fireball) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
		} else {
			Destroy (GetComponent<Rigidbody2D> ());
			transform.position = new Vector2 (0f, -6f);
		}

		if (!fireball) {
			if (transform.localScale.y > 0 && GameObject.Find (player).transform.localScale.x > 0) {

			} else if (transform.localScale.y < 0 && GameObject.Find (player).transform.localScale.x < 0) {

			} else {
				Flip ();
			}
		}

		if (facingRight) {
			x = playerLoc.x + charEnd;
		} else {
			x = playerLoc.x - charEnd;
		}
//		Debug.Log (playerLoc.x + " " + x);
		if (fireball) {
			if (transform.position.x > x && !fireballCollider && facingRight) {
				GetComponent<Collider2D> ().enabled = true;
				fireballCollider = true;
				GameObject.Find (player).GetComponent<Animator> ().SetBool ("fireball", false);
				fireballAnim = false;
				Debug.Log ("Disable");
			} else if (transform.position.x < x && !fireballCollider && !facingRight) {
				GetComponent<Collider2D> ().enabled = true;
				fireballCollider = true;
				GameObject.Find (player).GetComponent<Animator> ().SetBool ("fireball", false);
				fireballAnim = false;
				Debug.Log ("Disable");
			}
		}

		if (transform.position.x < start) {
			fireball = false;
		} else if (transform.position.x > end) {
			fireball = false;
		}

		if (fireballCollider) {
			GameObject.Find (player).GetComponent<Animator> ().SetBool ("fireball", false);
		}

	}

	void Flip() {
		facingRight = !facingRight;
		Vector2 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
		if (facingRight) {
			speed = maxSpeed;
		} else {
			speed = -maxSpeed;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		fireball = false;
	}
}
