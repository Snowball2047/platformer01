using UnityEngine;
using System.Collections;

public class fireballControler : MonoBehaviour {

    //Speed
	public float maxSpeed = 10f;
	float speed = 0f;

	bool fireball = false;
	bool facingRight = true;

    public string player = "nero";

    float charEnd = 1.8f;
	bool fireballCollider = false;

    //Level bounds
	public float start = 0f;
	public float end = 0f;

	bool fireballAnim = false;

	// Use this for initialization
	void Start () {
		speed = maxSpeed;
	}

	// Update is called once per frame
	void FixedUpdate () {

        //Tracks player location
        Vector2 playerLoc = GameObject.Find (player).transform.position;
		float x = playerLoc.x + 0.581f;
		float y = playerLoc.y - 0.2882f;

        //Tells Unity if fireball is active
		GetComponent<Animator> ().SetBool ("fireball", fireball);

        //Activates fireball
        if (Input.GetKeyDown (KeyCode.Space) && !fireball) {
            fireball = true;
            fireballAnim = true;
            //Starts player fieball anim
            GameObject.Find(player).GetComponent<Animator>().SetBool("fireball", true);
            //Sets fireball to player location
            transform.position = new Vector2 (x, y);
            //Adds Rigidbody2D
			transform.gameObject.AddComponent<Rigidbody2D> ();
            //Deactivates collider to pass through player
			GetComponent<Collider2D> ().enabled = false;
            
            fireballCollider = false;

		}

		if (fireball) {
            //Moves fireball forewards
            GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
		} else {
            //Disables Rigidbody2D
			Destroy (GetComponent<Rigidbody2D> ());
			transform.position = new Vector2 (0f, -6f);
		}
        
        //Checks if fireball is in the same direction as player when not active
		if (!fireball) {
			if (transform.localScale.y > 0 && GameObject.Find (player).transform.localScale.x > 0) {
                //Wow
			} else if (transform.localScale.y < 0 && GameObject.Find (player).transform.localScale.x < 0) {
                //Such empty
			} else {
				Flip ();
			}
		}

		if (facingRight) {
			x = playerLoc.x + charEnd;
		} else {
			x = playerLoc.x - charEnd;
		}

        //Debug.Log (playerLoc.x + " " + x);
		if (fireball) {
            //Enables collider when fireball leaves player
			if (transform.position.x > x && !fireballCollider && facingRight) {
                GetComponent<Collider2D> ().enabled = true;
				fireballCollider = true;
                //Stops player fireball anim
				GameObject.Find (player).GetComponent<Animator> ().SetBool ("fireball", false);
				fireballAnim = false;
			} else if (transform.position.x < x && !fireballCollider && !facingRight) {
				GetComponent<Collider2D> ().enabled = true;
				fireballCollider = true;
                //Stops player fireball anim
                GameObject.Find (player).GetComponent<Animator> ().SetBool ("fireball", false);
				fireballAnim = false;
			}
		}

        //Disables fireball when out of level bounds
		if (transform.position.x < start) {
			fireball = false;
		} else if (transform.position.x > end) {
			fireball = false;
		}

        //Fireball collision
		if (fireballCollider) {
			GameObject.Find (player).GetComponent<Animator> ().SetBool ("fireball", false);
		}

	}

    //Flips y-axis of fireball
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

    //Fireball collision
	void OnCollisionEnter2D(Collision2D coll) {
		fireball = false;
	}
}
