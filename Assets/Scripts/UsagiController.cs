using UnityEngine;
using System.Collections;

public class UsagiController : MonoBehaviour {
	
	public Transform LookTransform;
	
	public float walkspeed = 6.0f;
	public float runspeed = 8.0f;
	public float GroundHeight = 1.1f;
	private Vector2 faceDir;
	private float speed = 0;
	
	public Transform planet;
	private Transform player;
	public bool AlignToPlanet;
	public float gravityConstant = 9.8f;
	
	private Animator anim;
	private Rigidbody rigidBody;
	
	private Vector3 moveDir;
	private string usagiState;
	private float timerToGo;
	private float playerDistance;
	private Vector3 playerDirection;
	private Transform usagiBody;
	private Vector2[] directions = new Vector2[] {new Vector2(0f, -1f), new Vector2(-1f, 0f), new Vector2(1f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(-1f, -1f)};

	// Use this for initialization
	void Start () {
		player = GameObject.Find("SpaceBoy").transform;
		timerToGo = Random.Range(30f,250f);
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
		usagiState = "Idle";
		usagiBody = transform.Find("Body");
	}
	
	void Update () {
		switch (usagiState) {
		   case "Idle":
			    idle();
			    break;
		   case "Run":
			    run();
				break;
		   case "Walk":
			    walk();
			    break;
		}
		moveDir = new Vector3 (faceDir.x, 0, faceDir.y).normalized;
		playerDistance = Vector3.Distance(transform.position, player.position);
		playerDirection = (transform.position - player.position);
	}

	void idle() {
		faceDir.x = 0f;
		faceDir.y = 0f;
		if(timerToGo<=0) {
			usagiState = "Walk";
			timerToGo = Random.Range(30f,250f);
			randomDirection();
		}
		lookOut();
	}

	void walk() {
		speed = walkspeed;
		anim.SetBool("isHop", true);
		if(timerToGo<=0) {
			usagiState = "Idle";
			timerToGo = Random.Range(30f,250f);
			anim.SetBool("isHop", false);
		}
		lookOut();
	}

	void run() {
		faceDir.x = playerDirection.magnitude;
		faceDir.y = playerDirection.magnitude;
		speed = runspeed;
		if (playerDistance >= 10f) {
			usagiState = "Idle";
			anim.SetBool("isRun", false);
		}
	}

	void lookOut() {
		if (playerDistance <= 5f) {
			usagiState = "Run";
			anim.SetBool("isRun", true);
		} else {
			timerToGo -= 1f;
		}
	}
	
	void FixedUpdate () {
		RaycastHit groundedHit;
		bool grounded = Physics.Raycast(transform.position, -transform.up, out groundedHit, GroundHeight);
		
		if (grounded)
		{
			rigidBody.MovePosition(rigidBody.position + transform.TransformDirection(moveDir) * speed * Time.deltaTime);
		}

		if(faceDir.x <=-1f) { 
			usagiBody.GetComponent<Billboard>().reverseFace = true;
		}
		if(faceDir.x >=1f) { 
			usagiBody.GetComponent<Billboard>().reverseFace = false;
		}

		centerToPlanet ();
	}

	void centerToPlanet() {
		Vector3 toCenter = planet.position - transform.position;
		toCenter.Normalize();
		
		rigidBody.AddForce(toCenter * gravityConstant, ForceMode.Acceleration);
		rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
		
		if (AlignToPlanet) {
			Quaternion q = Quaternion.FromToRotation (transform.up, -toCenter);
			q = q * transform.rotation;
			transform.rotation = Quaternion.Slerp (transform.rotation, q, 1);
		}
	}

	void randomDirection() {
		Vector2 selectDirection = directions [Random.Range (0, directions.Length)];
		faceDir.x = selectDirection.x;
		faceDir.y = selectDirection.y;
	}
}
