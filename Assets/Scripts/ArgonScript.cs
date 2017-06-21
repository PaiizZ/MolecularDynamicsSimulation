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
	private float calculateValue;
	private float scalar;
	private float unitVector;
	private Vector3 velocityVector;
	private Vector3 momentumVector;
	private Vector3 forceVector;
	public GameController gameController;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		gameController = GameController.getInstance ();

		alpha = Random.Range (-10.0f, 10.0f);
		beta = Random.Range (-10.0f, 10.0f);
		gamma = Random.Range (-10.0f, 10.0f);

		calculateValue = Mathf.Pow (alpha, 2) + Mathf.Pow (beta, 2) + Mathf.Pow (gamma, 2);
		Debug.Log ("calculateValue: " + calculateValue);

		scalar = Mathf.Sqrt (calculateValue);
		Debug.Log ("scaleVector: " + scalar);

		unitVector = 1.0f / (scalar) * scalar;
		Debug.Log ("unitVector: " + unitVector);

		velocityVector = new Vector3(alpha * Mathf.Sqrt ((3 * R * T) / (massArgon*calculateValue)) , beta * Mathf.Sqrt ((3 * R * T) / (massArgon*calculateValue)), gamma * Mathf.Sqrt ((3 * R * T) / (massArgon*calculateValue)));
		Debug.Log ("velocity x:" + velocityVector.x +" y:" + velocityVector.y + " z:"+velocityVector.z);

		momentumVector = massArgon * velocityVector;
		Debug.Log ("momentum x:" + momentumVector.x +" y:" + momentumVector.y + " z:"+momentumVector.z);

//		rb.velocity = momentum ;
		for(int i = 0 ; i < 1 ; i++){
			Debug.Log ("x:" + gameController.gameObject.transform.GetChild(0).position.x+" y:" +  gameController.gameObject.transform.GetChild(0).position.y+" z:" +  gameController.gameObject.transform.GetChild(0).position.z);
		}
//		
	}

	public Vector3 getForce(ArgonScript obj){
		float wellDepth = 342.0f; //constant well depth of argon
		float diameter = 128.0f;  //constant diameter of argon
		float scalePosition = Mathf.Sqrt ( Mathf.Pow((this.transform.position.x - obj.transform.position.x),2) + 
										   Mathf.Pow((this.transform.position.y - obj.transform.position.y),2) + 
										   Mathf.Pow((this.transform.position.z - obj.transform.position.z),2) );
//		Debug.Log ("scalePosition : " + scalePosition);
		float A = 4*wellDepth*Mathf.Pow(diameter,12);
		float B = 4*wellDepth*Mathf.Pow(diameter,6);
		float energy = 12 * A * Mathf.Pow (scalePosition, -14) - 6 * B * Mathf.Pow (scalePosition, -8);
		float forceX = energy*(this.transform.position.x - obj.transform.position.x);
		float forceY = energy*(this.transform.position.y - obj.transform.position.y);
		float forceZ = energy*(this.transform.position.z - obj.transform.position.z);

		return new Vector3(forceX,forceY,forceZ);
	}

	 

//	void OnTriggerEnter(Collider other) {
//		Debug.Log ("xxx");
//		if (other.gameObject.CompareTag ("Argon")) {
//			ArgonScript otherArgon = (ArgonScript)other.gameObject.GetComponent ("ArgonScript");
//		
//			forceVector = getForce (otherArgon);
//			Debug.Log ("x:"+forceVector.x+" y:"+forceVector.y+" z:"+forceVector.x);
//            rb.AddForce(forceVector.x, forceVector.y, forceVector.z);
//		}
//	}

	// Update is called once per frame
	void Update () {
//		this.transform.GetChild (0);

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