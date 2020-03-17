using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseGameObject {

	public float JumpTime = 0.5f;

	Rigidbody rb;
	Animator ac;

	public float gravityScale = 1.0f;
	public static float globalGravity = -9.81f;

	void Awake () {
		rb = gameObject.GetComponent<Rigidbody> ();		
		ac = gameObject.GetComponent<Animator> ();	
	}


	void FixedUpdate() {
		HorizontalMove (); 
		VerticalMove ();
		Vector3 gravity = globalGravity * gravityScale * Vector3.up;
		rb.AddForce(gravity, ForceMode.Acceleration);
		ac.SetFloat("VelocityY", rb.velocity.y);
	}

	void HorizontalMove() {
		var horisontalAxis = Input.GetAxis("Horizontal");
		var targetPos = new Vector3 (ObjectPosition.x, ObjectPosition.y, Mathf.Clamp (ObjectPosition.z + horisontalAxis, -1, 1));
		ObjectPosition = Vector3.MoveTowards(ObjectPosition, targetPos, .4f);

	}

	void VerticalMove() {
		var verticalAxis = Input.GetAxis("Vertical");
		if (isGrounded && verticalAxis > 0) {
			StartCoroutine(Jump ());
		}
		if ((PlayerState == State.Run ||  Mathf.Abs(rb.velocity.y) > 0.1f) && verticalAxis < 0) {	

			if (isGrounded) {
				PlayerState = State.Slide;
				StartCoroutine (DoSlide ());
			} else {
				StartCoroutine(DoGrounded ());
			}
		}
	}

	IEnumerator DoGrounded() {
		gravityScale = 2f;
		yield return new WaitUntil (() => !isGrounded);
		gravityScale = 1f;
	}


	IEnumerator DoSlide() {
		var cc = gameObject.transform.GetChild (0).gameObject.GetComponent<CapsuleCollider> ();
		cc.center = new Vector3 (0, .26f, 0);
		cc.direction = 2;
		yield return new WaitForSeconds (0.21f);
		yield return new WaitUntil (() => ac.GetCurrentAnimatorStateInfo (0).IsName ("SLIDE"));
		cc.center = new Vector3 (0, .76f, 0);
		cc.direction = 1;
		PlayerState = State.Run;
	}

	enum State {
		Run,
		Slide,
		Jump,
		Falling
	}

	State _playerState = State.Run;

	State PlayerState {
		get { return _playerState; }
		set {
			_playerState = value; 
			ac.SetInteger ("PlayerState", (int)_playerState);
		}
	}
	void Update () {
	}

	void LateUpdate () {
		if (rb.velocity.y < -.25f) {
			PlayerState = State.Falling;
		}
		//print (Time.fixedTime + " rb.velocity.y :" + Mathf.Round(rb.velocity.y * 10));
		//print (Time.fixedTime + " PlayerState :" + PlayerState);
		//print (Time.fixedTime + " jumpWait: " + jumpWait);
		//print (Time.fixedTime + " isGrounded: " + isGrounded);
		if (isGrounded && !jumpWait && Mathf.Abs(rb.velocity.y) < .001f) {
			PlayerState = State.Run;			
		}
	}

	bool isGrounded;
	bool jumpWait = false;
	void OnCollisionStay(Collision other) {
		//print (Time.fixedTime + "Stay: " + other.gameObject.tag);
		if (other.gameObject.CompareTag ("Road")) {
			isGrounded = true;
			ac.SetBool ("isGrounded", isGrounded);
		}
	}

	void OnCollisionExit(Collision other) {
		//print (Time.fixedTime + "Exit: " + other.gameObject.tag);
		if (other.gameObject.CompareTag ("Road")) {
			isGrounded = false;
			ac.SetBool ("isGrounded", isGrounded);
		}
	}

	IEnumerator Jump() {
		PlayerState = State.Jump;
		jumpWait = true;
		gravityScale = -0.8f;
		yield return new WaitForSeconds (JumpTime);
		gravityScale = 1;
		jumpWait = false;
	}
}
