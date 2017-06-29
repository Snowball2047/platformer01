using UnityEngine;
using System.Collections;

public class playerControler : MonoBehaviour {

    public float maxSpeed = 10f;
    bool facingRight = true;

    Animator anim;

    //Ground check values
    bool grounded = false;
    public Transform GroundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    //Jump
    public float jumpForce = 700f;
    bool doJump = false;

    //Colliders
    GameObject uprightCollider;
    GameObject quadrupedalCollider;
    GameObject crouchCollider;

    //Player modifiers
    public bool upright = false;
    bool crouch = false;
	bool frozen = false;
	bool fireball = false;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

        //Char colliders
        uprightCollider = transform.Find("uprightCollider").gameObject;
        quadrupedalCollider = transform.Find("quadrupedalCollider").gameObject;
		crouchCollider = transform.Find ("crouchCollider").gameObject;
        uprightCollider.SetActive(false);
		crouchCollider.SetActive (false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Checks if player is on ground
        grounded = Physics2D.OverlapCircle(GroundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("grounded", grounded);

        //Movement of player, does not run if player is frozen
		if (!frozen) {
			float move = Input.GetAxis ("Horizontal");
			float speed = move * maxSpeed;
			if (upright) {
				speed = speed / 2;
			}

            //Moves player
            anim.SetFloat ("speed", Mathf.Abs (move));
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, GetComponent<Rigidbody2D> ().velocity.y);

            //Deactivates crouching anim
			if (speed != 0) {
                crouch = false;
			}

            //Flips player on y-axis if movement in opposite direction
            if (move > 0 && !facingRight) {
				Flip ();
			} else if (move < 0 && facingRight) {
				Flip ();
			}

			Crouch ();

            //Activates colliders when not crouched
			if (upright && !crouch) {
				uprightCollider.SetActive (true);
				quadrupedalCollider.SetActive (false);
				anim.SetBool ("upright", true);
			} else if (!crouch) {
				uprightCollider.SetActive (false);
				quadrupedalCollider.SetActive (true);
				anim.SetBool ("upright", false);
			}
		}
    }


    void Update()
    {
        //Prompts fireball animation
        if (Input.GetKeyDown (KeyCode.Space)) {
			fireball = true;
		}

		float jump = jumpForce;

        //Sets upright jump force
		if (upright) {
			jump = jump / 3;
			jump = jump * 2;
		}

        //Requests jump
		if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W)) {
			doJump = true;
		} else {
			doJump = false;
		}

        //Performs jump
        if (grounded && doJump)
        {
            //Checks if frozen
            if (!frozen) {
				anim.SetBool ("crouch", crouch);
				anim.SetBool ("grounded", false);
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jump));
				crouch = false;
			}
        }

        //Switch between upright and quadrupedal mode
        if (Input.GetKeyDown(KeyCode.F) && anim.GetFloat("speed") < 0.5)
        {
			if (!frozen) {
				Switch ();
				crouch = false;
			}
        }

        //Triggers crouch
		if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
			if (!frozen) {
				crouch = true;
				anim.SetBool ("crouch", crouch);
			}
		}

		//Testing triggers shock_hit
		if (Input.GetKeyDown(KeyCode.T)) {
			Switch ();
			anim.SetBool("shock_hit", true);
			frozen = true;
		}

		//Testing triggers fire_hit
		if (Input.GetKeyDown (KeyCode.Y)) {
			Switch ();
			anim.SetBool ("fire_hit", true);
			frozen = true;
		}

		//Testing triggers freeze
		if (Input.GetKeyDown (KeyCode.U)) {
			Switch ();
			anim.SetBool ("freeze", true);
			frozen = true;
		}
    }

    //Flips player on y-axis
    Flip()
    {
        crouch = false;
		facingRight = !facingRight;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
		anim.SetBool("crouch", crouch);
    }

    //Switch between upright and quadrupedal mode
    void Switch()
    {
        if(upright)
        {
            upright = false;
        }
        else
        {
            upright = true;
        }
		crouch = false;
    }

    //Sets crouch values in Unity to the same as in script
	void Crouch() {
		if (crouch) {
			anim.SetBool ("crouch", true);
			crouchCollider.SetActive (true);
			uprightCollider.SetActive (false);
			quadrupedalCollider.SetActive (false);

		} else {
			anim.SetBool ("crouch", false);
			crouchCollider.SetActive (false);
		}
	}
}
