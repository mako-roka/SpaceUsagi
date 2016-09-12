using UnityEngine;
using System.Collections;

public class UsagiController : MonoBehaviour {
	
	public Transform LookTransform;
	
	public float walkspeed = 6.0f;
	public float runspeed = 9.0f;
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
	private enum usagiAction {Idle, Run, Walk};
	private usagiAction usagiState;
	private float timerToGo;
	private float playerDistance;
	private Vector3 playerDirection;
	private Vector3 setDirection;
	private Vector3 fromCameraDirection;
	private Vector3 lastCameraDirection;
	private Transform usagiBody;
	private Vector2[] directions = new Vector2[] {new Vector2(0f, -1f), new Vector2(-1f, 0f), new Vector2(1f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(-1f, -1f)};

	// Use this for initialization
	void Start () {
		player = GameObject.Find("SpaceBoy").transform;
		timerToGo = Random.Range(30f,250f);
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
		usagiState = usagiAction.Idle;
		usagiBody = transform.Find("Body");
	}
	
	void Update () {
		switch (usagiState) {
		   case usagiAction.Idle:
			    idle();
			    break;
		   case usagiAction.Run:
			    run();
				break;
		   case usagiAction.Walk:
			    walk();
			    break;
		}
		playerDistance = Vector3.Distance(transform.position, player.position);
		playerDirection = (transform.position - player.position);
	}

	void idle() {
		moveDir = new Vector3 (0, 0, 0);
		timerToGo -= 1f;
		if(timerToGo<=0) {
			usagiState = usagiAction.Walk;
			timerToGo = Random.Range(30f,250f);
			randomDirection();
		}
		lookOut();
	}

	void walk() {
		speed = walkspeed;
		anim.SetBool("isHop", true);
		timerToGo -= 1f;
		if(timerToGo<=0) {
			usagiState = usagiAction.Idle;
			timerToGo = Random.Range(30f,250f);
			anim.SetBool("isHop", false);
		}
		lookOut();
	}

	void run() {
		setRunDirection ();
		speed = runspeed;
		anim.SetBool("isRun", true);
		if (playerDistance >= 10f) {
			usagiState = usagiAction.Idle;
			anim.SetBool("isRun", false);
		}
	}

	void lookOut() {
		if (playerDistance <= 5f) {
			usagiState = usagiAction.Run;
			anim.SetBool("isHop", false);
			anim.SetBool("isRun", true);
		}
	}
	
	void FixedUpdate () {
		RaycastHit groundedHit;
		bool grounded = Physics.Raycast(transform.position, -transform.up, out groundedHit, GroundHeight);

		if (grounded) {
		}

			fromCameraDirection = (Camera.main.transform.TransformDirection (transform.position).normalized - lastCameraDirection).normalized;
			fromCameraDirection.y = 0f;
		    if (fromCameraDirection.x != 0) {
			if (fromCameraDirection.x < 0f) {
				usagiBody.GetComponent<Billboard> ().reverseFace = true;
			} 
			if (fromCameraDirection.x > 0f) {
				usagiBody.GetComponent<Billboard> ().reverseFace = false;
			}
	      	}
			lastCameraDirection = Camera.main.transform.TransformDirection (transform.position).normalized;

		rigidBody.MovePosition(rigidBody.position + transform.TransformDirection(moveDir) * speed * Time.deltaTime);

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
		moveDir = new Vector3 (faceDir.x, 0, faceDir.y).normalized;
	}

	void setRunDirection() {
		faceDir.x = playerDirection.magnitude;
		faceDir.y = playerDirection.magnitude;
		moveDir = new Vector3 (faceDir.x, 0, faceDir.y).normalized;
	}

	float round(float number) {
		return Mathf.Round(number * 1000f) / 1000f;
	}
}
