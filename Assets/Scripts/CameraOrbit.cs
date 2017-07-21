using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
	    
	protected Transform _XForm_Camera;
	protected Transform _XForm_Parent;
	
	protected Vector3 _LocalRotation;
	protected float _CameraDistance = 10f;
	
	public float MouseSensitivity = 4f;
	public float ScrollSensitivity = 2f;
	public float OrbitDampening = 10f;
	public float ScrollDampening = 6f;
	
	public bool CameraDisabled = false;
	    
	// Use this for initialization
	void Start ()
	{
		this._XForm_Camera = this.transform;
		this._XForm_Parent = this.transform.parent;
	}
	
	// Update is called once per frame.
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.LeftShift))
			CameraDisabled = !CameraDisabled;
		
		if (!CameraDisabled) {
			//Rotation of the Camera based on Mouse Coordinates
			if (Input.GetAxis ("Mouse X") != 0 || Input.GetAxis ("Mouse Y") != 0) {
				_LocalRotation.x += Input.GetAxis ("Mouse X") * MouseSensitivity;
				_LocalRotation.y += Input.GetAxis ("Mouse Y") * MouseSensitivity;

				//Clamp the y Ratation to horizon and not flipping over at the top
				_LocalRotation.y = Mathf.Clamp (_LocalRotation.y, 0f, 90f);
			}
					
			//Zooming Input from our Mouse Scroll Wheel
			if (Input.GetAxis ("Mouse ScrollWheel") != 0f) {
				float ScrollAmount = Input.GetAxis ("Mouse ScrollWheel") * ScrollSensitivity;
				
				//Make camera zoom faster the futher away it is from the target
				ScrollAmount *= (this._CameraDistance * 0.3f);
				
				this._CameraDistance += ScrollAmount * -1f;
				//This makes camera go on closer than 1.5 meters from target, and on futher than 100 meters.
				this._CameraDistance = Mathf.Clamp (this._CameraDistance, 1.5f, 100f);
				
			}
		}
		
		//Actual Camera Rig Transformations
		Quaternion QT = Quaternion.Euler (_LocalRotation.y, _LocalRotation.x, 0);
		this._XForm_Parent.rotation = Quaternion.Lerp (this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);
		
		if (this._XForm_Camera.localPosition.z != this._CameraDistance * -1f) {
			this._XForm_Camera.localPosition = new Vector3 (0f, 0f, Mathf.Lerp (this._XForm_Camera.localPosition.z, 
											   this._CameraDistance * -1f, Time.deltaTime * ScrollDampening));
		}
	}
}
