using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrogenScript : MonoBehaviour
{
	//Physical of molecule
	public Rigidbody rb;
	//Controller every molecules
	private GameController gameController;
	// molar gas constant ( KJ/mol )
	private float R = 8.31447f * Mathf.Pow (10, -3);
	// temperature in kelvins (25+273)
	public float T = 298f;
	// mass of molecule argon
	private float massArgon = 1.00794f * Mathf.Pow (10, -3);
	// ( Kg / molecule )
	// attributes
	private float alpha;
	private float beta;
	private float gamma;
	private float calculateValue;
	private float scalar;
	private float velocity;
	//private Vector3 unitVector;
	private Vector3 randomVector;
	private Vector3 velocityVector;
	public Vector3 momentumVector;
	private Vector3 position;

	SpringJoint spring;

	// Use this for initialization
	void Start ()
	{
		gameController = GameController.getInstance ();

		rb = GetComponent<Rigidbody> ();

		alpha = Random.Range (-3.0f, 3.0f);
		beta = Random.Range (-3.0f, 3.0f);
		gamma = Random.Range (-3.0f, 3.0f);

		randomVector = new Vector3 (alpha, beta, gamma);

		calculateValue = Mathf.Pow (alpha, 2) + Mathf.Pow (beta, 2) + Mathf.Pow (gamma, 2);

		scalar = Mathf.Sqrt (calculateValue);

		//unitVector = (1 / scalar) * randomVector;

		velocity = Mathf.Sqrt ((3 * R * T) / (massArgon * calculateValue));

		velocityVector = velocity * randomVector;

		momentumVector = massArgon * velocityVector;

		//conectMolecule ();
		rb.velocity = momentumVector;

	}

	// Update is called once per frame
	void Update ()
	{
		//periodicBoundary();
		//this.transform.Translate (momentumVector*Time.deltaTime);
	}

	void conectMolecule ()
	{
		if (this.transform.parent.GetChild (2).position.x != this.transform.position.x) {
			// chemical bond formation suddenly pulls slightly closer together
//			float deltaX = this.transform.parent.GetChild (2).position.x - this.transform.position.x;
//			float deltaY = this.transform.parent.GetChild (2).position.y - this.transform.position.y;
//			float deltaZ = this.transform.parent.GetChild (2).position.z - this.transform.position.z;
//			this.transform.position = new Vector3 (
//				this.transform.position.x + 0.25f * deltaX,
//				this.transform.position.y + 0.25f * deltaY,
//				this.transform.position.z + 0.25f * deltaZ);
//				this.transform.parent.GetChild (2).position = new Vector3 (
//				this.transform.parent.GetChild (2).position.x - 0.25f * deltaX,
//				this.transform.parent.GetChild (2).position.y - 0.25f * deltaY,
//				this.transform.parent.GetChild (2).position.z - 0.25f * deltaZ);

			// create SpringJoint to implement covalent bond between these two atoms
			spring = this.gameObject.AddComponent<SpringJoint> ();
			spring.connectedBody = this.transform.parent.GetChild (2).gameObject.GetComponent<Rigidbody> ();
			spring.anchor = new Vector3 (0, 0, 0);
			spring.connectedAnchor = new Vector3 (0, 0, 0);
			spring.spring = 10;
			spring.minDistance = 0;
			spring.maxDistance = 0;
			spring.tolerance = 0.025f;
			spring.breakForce = Mathf.Infinity;
			spring.breakTorque = Mathf.Infinity;
			spring.enableCollision = false;
			spring.enablePreprocessing = true;
		} 
//		else if (this.transform.parent.GetChild (1).position.x != this.transform.position.x) {
//			// chemical bond formation suddenly pulls slightly closer together
////			float deltaX = this.transform.parent.GetChild (1).position.x - this.transform.position.x;
////			float deltaY = this.transform.parent.GetChild (1).position.y - this.transform.position.y;
////			float deltaZ = this.transform.parent.GetChild (1).position.z - this.transform.position.z;
////			this.transform.position = new Vector3 (
////				this.transform.position.x + 0.25f * deltaX,
////				this.transform.position.y + 0.25f * deltaY,
////				this.transform.position.z + 0.25f * deltaZ);
////			this.transform.parent.GetChild (2).position = new Vector3 (
////				this.transform.parent.GetChild (2).position.x - 0.25f * deltaX,
////				this.transform.parent.GetChild (2).position.y - 0.25f * deltaY,
////				this.transform.parent.GetChild (2).position.z - 0.25f * deltaZ);
//
//			// create SpringJoint to implement covalent bond between these two atoms
//			spring = this.gameObject.AddComponent<SpringJoint> ();
//			spring.connectedBody = this.transform.parent.GetChild (1).gameObject.GetComponent<Rigidbody> ();
//			spring.anchor = new Vector3 (0, 0, 0);
//			spring.connectedAnchor = new Vector3 (0, 0, 0);
//			spring.spring = 10;
//			spring.minDistance = 0;
//			spring.maxDistance = 0;
//			spring.tolerance = 0.025f;
//			spring.breakForce = Mathf.Infinity;
//			spring.breakTorque = Mathf.Infinity;
//			spring.enableCollision = false;
//			spring.enablePreprocessing = true;
//		}
	}
}
