using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float JumpTime = 0.5f;

	Vector3 PlayerPos { 
		get { return gameObject.transform.position; } 
		set { gameObject.transform.position = value; }	
	}
	Rigidbody rb;
	Animator ac;

	[SerializeField]
	GameObject camera;

	Vector3 cameraDistance;

	Vector3 CameraPos {
		get { return camera.transform.position; }
		set { camera.transform.position = value; }
	}

	public float gravityScale = 1.0f;
	public static float globalGravity = -9.81f;

	// Use this for initialization
	void Awake () {
		rb = gameObject.GetComponent<Rigidbody> ();		
		ac = gameObject.GetComponent<Animator> ();	
		cameraDistance = CameraPos - PlayerPos;
	}


	void FixedUpdate() {
		HorizontalMove ();
		VerticalMove ();
		Vector3 gravity = globalGravity * gravityScale * Vector3.up;
		rb.AddForce(gravity, ForceMode.Acceleration);
		CameraPos = Vector3.MoveTowards(CameraPos, PlayerPos+cameraDistance, .4f);
		ac.SetFloat("VelocityY", rb.velocity.y);
	}

	void HorizontalMove() {
		var horisontalAxis = Input.GetAxis("Horizontal");
		var targetPos = new Vector3 (PlayerPos.x, PlayerPos.y, Mathf.Clamp (PlayerPos.z + horisontalAxis, -1, 1));
		PlayerPos = Vector3.MoveTowards(PlayerPos, targetPos, .4f);

	}

	void VerticalMove() {
		var verticalAxis = Input.GetAxis("Vertical");

		if (gravityScale > 0 && Mathf.Abs(rb.velocity.y) < 0.1f && isGrounded && verticalAxis > 0) {
			StartCoroutine(Jump ());
		}
		if ((PlayerState == State.Run || PlayerState == State.Jump) && verticalAxis < 0) {			
			StartCoroutine (DoSlide ());
		}
	}



	IEnumerator DoSlide() {
		if (!isGrounded) {
			gravityScale = 1.5f;
			ac.SetTrigger ("Slide");
			yield return new WaitUntil (() => !isGrounded);
			gravityScale = 4f;
		} else {
			//ac.SetTrigger ("Slide");
		}
		var cc = gameObject.transform.GetChild (0).gameObject.GetComponent<CapsuleCollider> ();
		cc.center = new Vector3 (0, .26f, 0);
		cc.direction = 2;
		yield return new WaitUntil (() => ac.GetCurrentAnimatorStateInfo (0).IsName ("SLIDE"));
		cc.center = new Vector3 (0, .76f, 0);
		cc.direction = 1;
	}

	enum State {
		Run,
		Slide,
		Jump,
		Falling
	}

	State PlayerState= State.Run;

	void Update () {
		print ("Update: " + isGrounded);
	}

	void LateUpdate () {
		print ("LateUpdate: " + isGrounded);
	}

	bool isGrounded;
	bool jumpWait = false;
	void OnCollisionStay(Collision other) {
		print (other.gameObject.tag);
		if (other.gameObject.CompareTag ("Road")) {
			isGrounded = true;
			ac.SetBool ("isGrounded", isGrounded);
			if (!jumpWait) {
				PlayerState = State.Run;
			}
		}
	}

	void OnCollisionExit(Collision other) {
		if (other.gameObject.CompareTag ("Road")) {
			isGrounded = false;
			ac.SetBool ("isGrounded", isGrounded);
		}
	}

	IEnumerator Jump() {
		PlayerState = State.Jump;
		jumpWait = true;
		gravityScale = -2f;
		yield return new WaitForSeconds (JumpTime);
		gravityScale = 1;
		yield return new WaitForSeconds (0.2f);
		PlayerState = State.Falling;
		jumpWait = false;
	}
}
