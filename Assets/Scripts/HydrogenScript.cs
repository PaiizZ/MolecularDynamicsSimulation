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
	//	private float scalar;
	private float velocity;
	private float time;
	//private Vector3 unitVector;
	private Vector3 randomVector;
	private Vector3 velocityVector;
	public Vector3 momentumVector;
	private Vector3 position;
	private Vector3 forceVector;
	private int numberOfWater;

	SpringJoint spring;

	// Use this for initialization
	void Start ()
	{
		gameController = GameController.getInstance ();

		rb = GetComponent<Rigidbody> ();

		this.numberOfWater = gameController.getNumberOxygen ();

		alpha = Random.Range (-3.0f, 3.0f);
		beta = Random.Range (-3.0f, 3.0f);
		gamma = Random.Range (-3.0f, 3.0f);

		randomVector = new Vector3 (alpha, beta, gamma);

		calculateValue = Mathf.Pow (alpha, 2) + Mathf.Pow (beta, 2) + Mathf.Pow (gamma, 2);

//		scalar = Mathf.Sqrt (calculateValue);

		velocity = Mathf.Sqrt ((3 * R * T) / (massArgon * calculateValue));

		velocityVector = velocity * randomVector;

		momentumVector = massArgon * velocityVector;

		conectMolecule ();

		//rb.velocity = momentumVector;

	}

	// Update is called once per frame
	void Update ()
	{
		this.time = Time.deltaTime * Mathf.Pow (10, -12);
		if (this.transform.parent.GetChild (1).position.x != this.transform.position.x) {
			momentumVector = momentumVector + (0.5f * time * forceVector);
			forceVector = this.transform.parent.gameObject.GetComponent<OxygenScript> ().forceH1;
			momentumVector = momentumVector + (0.5f * time * forceVector);
			rb.velocity = momentumVector;
		} else if (this.transform.parent.GetChild (0).position.x != this.transform.position.x) {
			momentumVector = momentumVector + (0.5f * time * forceVector);
			forceVector = this.transform.parent.gameObject.GetComponent<OxygenScript> ().forceH2;
			momentumVector = momentumVector + (0.5f * time * forceVector);
			rb.velocity = momentumVector;
		}
		electrostatic ();
	}

	void electrostatic(){
		float Kspring = 9 * Mathf.Pow (10, 15); // (KJnm/c^2)
		float elementaryCharge = 1.602f * Mathf.Pow (10,-19);// c
		float electricChargeOxygen = -0.82f * elementaryCharge; // c
		float electricChargeHydrogen = 0.41f * elementaryCharge; // c
		float numberHydrogeninWater = 2 ;
		List<Vector3> posAtoms = new List<Vector3> ();
		Vector3 position = this.transform.position ; 
		for(int i = 0 ; i < numberOfWater ; i ++){
			Transform transformChild = this.transform.parent.parent.GetChild (i);

			if(position != transformChild.position){
				posAtoms.Add (transformChild.position);
			}
			for(int j = 0 ; j < numberHydrogeninWater  ; j ++){
				if (position != transformChild.GetChild (j).position) {
					posAtoms.Add (transformChild.GetChild (j).position);
				}
			}
		}

		foreach (Vector3 tempPos in posAtoms) {
			Vector3 unitVector = tempPos - position;
			Vector3 springForce = ( (Kspring * electricChargeOxygen * electricChargeHydrogen) 
				/ Mathf.Pow(Mathf.Pow(tempPos.x-position.x,2)+Mathf.Pow(tempPos.y-position.y,2)+Mathf.Pow(tempPos.z-position.z,2),1.5f) ) * unitVector;
			rb.AddForce (springForce);
		}
	}

	void conectMolecule ()
	{
		if (this.transform.parent.GetChild (1).position.x != this.transform.position.x) {
			// create SpringJoint to implement covalent bond between these two atoms
			spring = this.gameObject.AddComponent<SpringJoint> ();
			spring.connectedBody = this.transform.parent.GetChild (1).gameObject.GetComponent<Rigidbody> ();
			spring.anchor = new Vector3 (0, 0, 0);
			spring.connectedAnchor = new Vector3 (0, 0, 0);
			spring.spring = 10;
			spring.minDistance = 0.1633f;
			spring.maxDistance = 0.1633f;
			spring.tolerance = 0.025f;
			spring.breakForce = Mathf.Infinity;
			spring.breakTorque = Mathf.Infinity;
			spring.enableCollision = false;
			spring.enablePreprocessing = true;
		} else if (this.transform.parent.GetChild (0).position.x != this.transform.position.x) {
			// create SpringJoint to implement covalent bond between these two atoms
			spring = this.gameObject.AddComponent<SpringJoint> ();
			spring.connectedBody = this.transform.parent.GetChild (0).gameObject.GetComponent<Rigidbody> ();
			spring.anchor = new Vector3 (0, 0, 0);
			spring.connectedAnchor = new Vector3 (0, 0, 0);
			spring.spring = 10;
			spring.minDistance = 0.1633f;
			spring.maxDistance = 0.1633f;
			spring.tolerance = 0.025f;
			spring.breakForce = Mathf.Infinity;
			spring.breakTorque = Mathf.Infinity;
			spring.enableCollision = false;
			spring.enablePreprocessing = true;
		}
	}
}
