using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public Camera cam;

    [Header("Jumping Attributes")]
    public Transform feetPoint;
    public float groundCheckDist;
    public float jumpForce;
    public int numJumps;
    public LayerMask groundLayer;

    private Animator anim;
    private AudioSource audioSource;
    private Rigidbody2D rb2d;
    private SpriteRenderer sr;
    private float speed = 5f;
    private bool facingRight = true;
    private bool jumped;
    private int timesJumped;

	// Use this for initialization
	void Start ()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        cam.transform.position = new Vector3(rb2d.transform.position.x, cam.transform.position.y, cam.transform.position.z);
        timesJumped = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float move = Input.GetAxis("Horizontal");
        if (move != 0) {
            rb2d.transform.Translate(new Vector3(1, 0, 0) * move * speed * Time.deltaTime);
            cam.transform.position = new Vector3(rb2d.transform.position.x, cam.transform.position.y, cam.transform.position.z);
            facingRight = move > 0;
        }
        
        sr.flipX = !facingRight;

        if (Input.GetButtonDown("Jump") && ++timesJumped < numJumps)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            rb2d.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
            // play sound
            anim.SetBool("Jump", true);
            audioSource.Play();
        }

        // animator parameters
        anim.SetFloat("Speed", Mathf.Abs(move));


        // metodo 1 para detectar contacto
        DetectPlayerTouchingGround();
	}

    /// <summary>
    /// Metodo para detectar si el jugador esta en contacto con algun objeto bajo sus pies.
    /// </summary>
    private void DetectPlayerTouchingGround()
    {
        Collider2D col = Physics2D.OverlapBox(feetPoint.position, Vector2.one * groundCheckDist, 0f, groundLayer);
        if (col != null)
        {
            jumped = false;
            timesJumped = 0;
        }
        else
        {
            jumped = true;
        }
        // set animator parameter
        anim.SetBool("Jump", jumped);
    }
}
