using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public float angleX = 33;
	public float angleY = 1;
	public float angleZ = 1;
	//public float posX = 0;
	//public float posY = 0;
	//public float posZ = 0;
	
	[SerializeField]
	private Transform target;
	
	[SerializeField]
	private Vector3 offsetPosition;
	
	[SerializeField]
	private Space offsetPositionSpace = Space.Self;
	
	[SerializeField]
	private bool lookAt = false;
	
	private void Update()
	{
		Refresh();
	}
	
	public void Refresh()
	{
		if(target == null)
		{
			Debug.LogWarning("Missing target ref !", this);
			
			return;
		}
		
		// compute position
		if(offsetPositionSpace == Space.Self)
		{
			transform.position = target.TransformPoint(offsetPosition);
		}
		else
		{
			//Vector3 velocity = Vector3.zero;
			//Vector3 forward = target.transform.forward * 10.0f;
			//Vector3 needPos = target.transform.position - forward;
			//transform.position = Vector3.SmoothDamp(transform.position, needPos, ref velocity,0.05f);
			transform.position = target.position + offsetPosition;
		}
		
		// compute rotation
		if(lookAt)
		{
			transform.LookAt(target);
		}
		else
		{
			transform.rotation = target.rotation * Quaternion.Euler(angleX, angleY, angleZ);
		}
	}
	
}
