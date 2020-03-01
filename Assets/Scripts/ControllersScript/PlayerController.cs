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
	float distToGround  ;

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
		gravityScale = 5f;
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
		gravityScale = -3f;
		yield return new WaitForSeconds (JumpTime);
		gravityScale = 1;
		jumpWait = false;
	}
}
