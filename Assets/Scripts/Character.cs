using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public Camera cam;
    public Transform feetPoint;
    public float groundCheckDist;
    public float jumpForce;
    public LayerMask groundLayer;

    private Animator anim;
    private AudioSource audioSource;
    private Rigidbody2D rb2d;
    private SpriteRenderer sr;
    private float speed = 5f;
    private bool facingRight = true;
    private bool jumped;

	// Use this for initialization
	void Start ()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        cam.transform.position = new Vector3(rb2d.transform.position.x, cam.transform.position.y, cam.transform.position.z);
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

        if (Input.GetButtonDown("Jump") && !jumped)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            rb2d.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
            // play sound
            anim.SetBool("Jump", true);
            audioSource.Play();
        }

        // animator parameters
        anim.SetFloat("Speed", Mathf.Abs(move));


        // detect ground contact
        DetectPlayerTouchingGround();
	}

    private void DetectPlayerTouchingGround()
    {
        Collider2D col = Physics2D.OverlapBox(feetPoint.position, Vector2.one * groundCheckDist, 0f, groundLayer);
        if (col != null) jumped = false;
        else jumped = true;
        anim.SetBool("Jump", jumped);
    }
}
