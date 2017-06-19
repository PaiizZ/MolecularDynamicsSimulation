using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgonScript : MonoBehaviour {
	private Rigidbody rb;
	private float R = 8.31447f ; //molar gas constant ( J/(mol*Kg) )
	private float T = 298f ; // temperature in kelvins (25+273)
	private float massArgon = 39.948f * Mathf.Pow(10,-3) ; // ( Kg/mol )
	private float alpha;
	private float beta;
	private float gamma;
	private float scaleVector;
	private float unitVector;
	private float velocity;
	private float momentum;
	private Vector3 forceVector;
	GameObject otherObject = null;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		alpha = Random.Range (-10.0f, 10.0f);
		beta = Random.Range (-10.0f, 10.0f);
		gamma = Random.Range (-10.0f, 10.0f);
		scaleVector = Mathf.Sqrt ( Mathf.Pow(alpha ,2) + Mathf.Pow(beta ,2) + Mathf.Pow(gamma ,2) );
//		Debug.Log ("scaleVector: " + scaleVector);
		unitVector = 1.0f / (Mathf.Sqrt (Mathf.Pow (alpha, 2) + Mathf.Pow (beta, 2) + Mathf.Pow (gamma, 2))) * scaleVector;
//		Debug.Log ("unitVector: " + unitVector);
		velocity = Mathf.Sqrt ( 3 * R * T / massArgon ) * unitVector ;
		Debug.Log ("velocity: " + velocity);
		momentum = massArgon * velocity;
		Debug.Log ("momentum: " + momentum);
//		rb.velocity = new Vector3(
//			Random.Range(-velocity, velocity),
//			Random.Range(-velocity, velocity),
//			Random.Range(-velocity, velocity));
//		
	}

	public Vector3 getForce(ArgonScript obj){
		float wellDepth = 342.0f;
		float diameter = 128.0f;
		float scalePosition = Mathf.Sqrt ( Mathf.Pow((obj.transform.position.x - this.transform.position.x),2) + 
										   Mathf.Pow((obj.transform.position.y - this.transform.position.y),2) + 
										   Mathf.Pow((obj.transform.position.z - this.transform.position.z),2) );
//		Debug.Log ("scalePosition : " + scalePosition);
		float A = 4*wellDepth*Mathf.Pow(diameter,12);
		float B = 4*wellDepth*Mathf.Pow(diameter,6);
		float energy = 12 * A * Mathf.Pow (scalePosition, -14) - 6 * B * Mathf.Pow (scalePosition, -8);
		float forceX = energy*(obj.transform.position.x - this.transform.position.x);
		float forceY = energy*(obj.transform.position.y - this.transform.position.y);
		float forceZ = energy*(obj.transform.position.z - this.transform.position.z);

		return new Vector3(forceX,forceY,forceZ);
	}

	 

	void OnTriggerEnter(Collider other) {
		Debug.Log ("xxx");
		if (other.gameObject.CompareTag ("Argon")) {
			ArgonScript otherArgon = (ArgonScript)other.gameObject.GetComponent ("ArgonScript");
		
			forceVector = getForce (otherArgon);
			Debug.Log ("x:"+forceVector.x+" y:"+forceVector.y+" z:"+forceVector.x);
            rb.AddForce(forceVector.x, forceVector.y, forceVector.z);
		}
	}

	// Update is called once per frame
	void Update () {

	}













//	void OnTriggerEnter(Collider other)
//	{
//		if (partner == null && other.gameObject.CompareTag("Argon"))
//		{
//			ArgonScript otherArgon = (ArgonScript)other.gameObject.GetComponent("Argon");
//
//			if (otherArgon.partner == null) // two free radicals meet and form covalent bond
//			{
//				partner = other.gameObject;
//				otherArgon.partner = this.gameObject;
//
//				// chemical bond formation suddenly pulls slightly closer together
//				float deltaX = partner.transform.position.x - this.transform.position.x;
//				float deltaY = partner.transform.position.y - this.transform.position.y;
//				float deltaZ = partner.transform.position.z - this.transform.position.z;
//				this.transform.position = new Vector3(
//					this.transform.position.x + 0.25f * deltaX,
//					this.transform.position.y + 0.25f * deltaY,
//					this.transform.position.z + 0.25f * deltaZ);
//				partner.transform.position = new Vector3(
//					partner.transform.position.x - 0.25f * deltaX,
//					partner.transform.position.y - 0.25f * deltaY,
//					partner.transform.position.z - 0.25f * deltaZ);

				// create SpringJoint to implement covalent bond between these two atoms
//				springJoint = this.gameObject.AddComponent<SpringJoint>();
//				springJoint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
//				springJoint.anchor = new Vector3(0, 0, 0);
//				springJoint.connectedAnchor = new Vector3(0, 0, 0);
//				springJoint.spring = 10;
//				springJoint.minDistance = 0.0f;
//				springJoint.maxDistance = 0.0f;
//				springJoint.tolerance = 0.025f;
//				springJoint.breakForce = Mathf.Infinity;
//				springJoint.breakTorque = Mathf.Infinity;
//				springJoint.enableCollision = false;
//				springJoint.enablePreprocessing = true;
//			}
//		}
//		if (other.gameObject.CompareTag("Wall"))
//		{
//			audioSource.PlayOneShot(audioClipBallBounce, 0.25f);
//		}
//	}
}