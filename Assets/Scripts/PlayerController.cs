using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Transform LookTransform;
	
	public float walkspeed = 6.0f;
	public float GroundHeight = 1.1f;
	private float slidespeed = 15.0f;
	private Vector2 faceDir;
	private float speed = 0;

	public Transform planet;
	public bool AlignToPlanet;
	public float gravityConstant = 9.8f;

	private Animator anim;
	private Rigidbody rigidBody;
	
	private Vector3 moveDir;
	private bool isGrab = false;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
		faceDir.x = 1f;
	}

	void Update () {
		if (!isGrab) {
			moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized;
		} else {
			moveDir = new Vector3 (faceDir.x, 0, faceDir.y).normalized;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		RaycastHit groundedHit;
		bool grounded = Physics.Raycast(transform.position, -transform.up, out groundedHit, GroundHeight);
		
		//if (grounded)
		//{
			/*// Calculate how fast we should be moving
			Vector3 forward = Vector3.Cross(transform.up, -LookTransform.right).normalized;
			Vector3 right = Vector3.Cross(transform.up, LookTransform.forward).normalized;
			Vector3 targetVelocity = (forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal")) * speed;
			
			Vector3 velocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
			velocity.y = 0;
			velocity = transform.TransformDirection(velocity);
			Vector3 velocityChange = transform.InverseTransformDirection(targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			//velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			//velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);
			velocityChange = transform.TransformDirection(velocityChange);
			
			GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);*/
			if(!isGrab) {
			     if(Mathf.Abs(Input.GetAxisRaw ("Horizontal")) >= 0.1 || Mathf.Abs(Input.GetAxisRaw ("Vertical")) >= 0.1) {
			       anim.SetBool ("isWalk", true);
					if(Mathf.Abs(Input.GetAxisRaw ("Horizontal")) >= 0.1) {
					   faceDir.x = Input.GetAxisRaw ("Horizontal");
					}
					faceDir.y = Input.GetAxisRaw ("Vertical");
					speed = walkspeed;
			     } else {
				    anim.SetBool ("isWalk", false);
					speed = 0f;
			     }

				if(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")){
					isGrab = true;
					anim.SetBool ("isWalk", false);
					slidespeed = 40.0f;
			     }
			} else {
				slidespeed -= 1f;
				speed = slidespeed;
				anim.SetFloat("Slide", slidespeed);
				if(slidespeed <= 0f) {
					isGrab = false;
				}
			}
		if (faceDir.x != 0) {
			transform.localScale = new Vector3 (0.44f * faceDir.x, transform.localScale.y, transform.localScale.z);
		}
			rigidBody.MovePosition(rigidBody.position + transform.TransformDirection(moveDir) * speed * Time.deltaTime);
		//}
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
} 
