using UnityEngine;
using System.Collections;

public class playerControler : MonoBehaviour {

    public float maxSpeed = 10f;
    bool facingRight = true;

    Animator anim;

    bool grounded = false;
    public Transform GroundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    public float jumpForce = 700f;
    public bool upright = false;
    GameObject uprightCollider;
    GameObject quadrupedalCollider;
	GameObject crouchCollider;
	bool doJump = false;
	bool crouch = false;
	bool frozen = false;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        uprightCollider = transform.Find("uprightCollider").gameObject;
        quadrupedalCollider = transform.Find("quadrupedalCollider").gameObject;
		crouchCollider = transform.Find ("crouchCollider").gameObject;
        uprightCollider.SetActive(false);
		crouchCollider.SetActive (false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(GroundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("grounded", grounded);
		float move = Input.GetAxis("Horizontal");
        float speed = move * maxSpeed;
		if (upright && ! frozen)
        {
            speed = speed / 2;
        }
		anim.SetFloat ("speed", Mathf.Abs (move));
		if (!frozen) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, GetComponent<Rigidbody2D> ().velocity.y);
		}
		if (speed != 0) {
			crouch = false;
		}
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }
		Crouch();
		if (upright && ! crouch) {
			uprightCollider.SetActive (true);
			quadrupedalCollider.SetActive (false);
			anim.SetBool("upright", true);
		} else if (! crouch) {
			uprightCollider.SetActive(false);
			quadrupedalCollider.SetActive(true);
			anim.SetBool("upright", false);
		}
    }


    void Update()
    {
		float jump = jumpForce;
		if (upright) {
			jump = jump / 3;
			jump = jump * 2;
		}
		if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W)) {
			doJump = true;
		} else {
			doJump = false;
		}
		if (grounded && doJump)
        {
			if (!frozen) {
				anim.SetBool ("crouch", crouch);
				anim.SetBool ("grounded", false);
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jump));
				crouch = false;
			}
        }

        if (Input.GetKeyDown(KeyCode.F) && anim.GetFloat("speed") < 0.5)
        {
			if (!frozen) {
				Switch ();
				crouch = false;
			}
        }
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

    void Flip()
    {
		crouch = false;
		facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
		anim.SetBool("crouch", crouch);
    }

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
