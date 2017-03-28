using UnityEngine;
using System.Collections;

public class playerControler : MonoBehaviour {

    public float maxSpeed = 10f;
    bool facingRight = true;

    Animator anim;

    bool grounded = false;
    public Transform GroundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    public float jumpForce = 700f;
    public bool upright = false;
    GameObject uprightCollider;
    GameObject quadrupedalCollider;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        uprightCollider = transform.Find("uprightCollider").gameObject;
        quadrupedalCollider = transform.Find("quadrupedalCollider").gameObject;
        uprightCollider.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(GroundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("grounded", grounded);
        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("speed", Mathf.Abs(move));
        float speed = move * maxSpeed;
        if (upright)
        {
            speed = speed / 2;
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

    }


    void Update()
    {
		float jump = jumpForce;
		if (upright) {
			jump = jump / 3;
			jump = jump * 2;
		}
			
        if (grounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetBool("grounded", false);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jump));
        }

        if (Input.GetKeyDown(KeyCode.Space) && anim.GetFloat("speed") < 0.5)
        {
            Switch();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Switch()
    {
        if(upright)
        {
            upright = false;
            anim.SetBool("upright", false);
            uprightCollider.SetActive(false);
            quadrupedalCollider.SetActive(true);
        }
        else
        {
            upright = true;
            anim.SetBool("upright", true);
            uprightCollider.SetActive(true);
            quadrupedalCollider.SetActive(false);
        }
    }
}
