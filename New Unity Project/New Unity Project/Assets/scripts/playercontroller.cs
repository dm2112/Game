﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour {

	//movement variables
	public float maxSpeed;

	//jumping variables
	bool grounded = false;
	float groundCheckRadius = 0.1f;
	public LayerMask groundLayer;
	public Transform groundCheck;
	public float jumpHeight;

	public Rigidbody2D rb;
	public Animator myAnim;
	bool facingRight;

	//shooting
	public Transform launch;
	public GameObject bubble;
	float fireRate = 0.25f;
	float nextFire = 0.0f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		myAnim = GetComponent<Animator>();

		facingRight = true;
	}

	// Update is called at a fixed interval
	void Update()
	{
		if (grounded && Input.GetAxis ("Jump") > 0) {
			grounded = false;
			myAnim.SetBool ("isGrounded", grounded);
			rb.AddForce (new Vector2 (0, jumpHeight));
		}

		//player shooting
		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			fireBubble ();
		}
			
	}


	void FixedUpdate () {

		//check if we are grounded, if no then we are falling
		grounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);
		myAnim.SetBool ("isGrounded", grounded);

		myAnim.SetFloat ("verticalSpeed", rb.velocity.y);

		float move = Input.GetAxis("Horizontal");
		myAnim.SetFloat ("speed", Mathf.Abs (move));

		rb.velocity = new Vector2 (move*maxSpeed, rb.velocity.y);

		if (move>0&&!facingRight) {
			flip();
		} else if (move<0&&facingRight) {
			flip();
		}
	}

	void flip()
	{
		facingRight=!facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	void fireBubble()
	{
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			if (facingRight) {
				Instantiate (bubble, launch.position, Quaternion.Euler (0, 0, 0));
			}
		 	else if (!facingRight) {
				Instantiate (bubble, launch.position, Quaternion.Euler (0, 0, 180f));
				bubble.transform.Rotate(new Vector3(180f,0,0));
			}
		}
	}
}