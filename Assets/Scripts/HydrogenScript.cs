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
	private float massHydrogen = 1.00794f * Mathf.Pow (10, -3); // ( Kg / molecule )

	// attributes
	private float alpha;
	private float beta;
	private float gamma;
	private float calculateValue;
	private float velocity;
	private float time;
	private Vector3 randomVector;
	private Vector3 velocityVector;
	public Vector3 momentumVector;
	private Vector3 position;
	private Vector3 forceVector;
	private int numberOfWater;

	// attributes spring
	SpringJoint spring;

	// Use this for initialization
	void Start ()
	{
		gameController = GameController.getInstance ();

		rb = GetComponent<Rigidbody> ();

		this.numberOfWater = gameController.getNumberOxygen ();

		this.position = this.transform.position; 

		alpha = Random.Range (-3.0f, 3.0f);
		beta = Random.Range (-3.0f, 3.0f);
		gamma = Random.Range (-3.0f, 3.0f);

		randomVector = new Vector3 (alpha, beta, gamma);

		calculateValue = Mathf.Pow (alpha, 2) + Mathf.Pow (beta, 2) + Mathf.Pow (gamma, 2);

		velocity = Mathf.Sqrt ((3 * R * T) / (massHydrogen * calculateValue));

		velocityVector = velocity * randomVector;

		momentumVector = massHydrogen * velocityVector;

		conectMolecule ();
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

	//calculate an electrostatic force every pair
	void electrostatic ()
	{
		// Spring constant = 6.02f * Mathf.Pow (10, 23) * 9 * Mathf.Pow (10, 15); // (KJnm/c^2)
		// elementaryCharge = 1.602f * Mathf.Pow (10, -19);// c
		// electricChargeOxygen = -0.82f * elementaryCharge; // c
		// electricChargeHydrogen = 0.41f * elementaryCharge; // c
		float electricChargeO_O = 54f * Mathf.Pow(1.33f,2); // Spring constant * electricChargeOxygen + electricChargeOxygen
		float electricChargeH_H = 54f * Mathf.Pow(0.665f,2); // Spring constant * electricChargeHydrogen + electricChargeHydrogen
		float electricChargeO_H = 54f * 0.665f * -1.33f; // Spring constant * electricChargeOxygen + electricChargeHydrogen
		float numberHydrogeninWater = 2 ;

		List<Vector3> posAtoms = new List<Vector3> ();
		List<float> electricChargeOfAtoms = new List<float> ();

		for (int i = 0; i < numberOfWater; i++) {
			Transform transformChild = this.transform.parent.parent.GetChild (i);

			if (this.position != transformChild.position) {
				posAtoms.Add (transformChild.position);
				electricChargeOfAtoms.Add(electricChargeH_H);
			}
			for (int j = 0; j < numberHydrogeninWater; j++) {
				if (this.position != transformChild.GetChild (j).position) {
					posAtoms.Add (transformChild.GetChild (j).position);
					electricChargeOfAtoms.Add(electricChargeO_H);
				}
			}
		}

		for(int i = 0 ; i < electricChargeOfAtoms.Count ; i++){
			Vector3 posAtomInList = posAtoms [i];
			Vector3 unitVector = posAtomInList - this.position;
			Vector3 springForce = (electricChargeOfAtoms[i]
				/ Mathf.Pow (Mathf.Pow (posAtomInList.x - position.x, 2) + Mathf.Pow (posAtomInList.y - position.y, 2) + Mathf.Pow (posAtomInList.z - position.z, 2), 1.5f)) * unitVector;
			rb.AddForce (springForce*Mathf.Pow(10,-3));
		}
	}

	// combine each hydrogen atom
	void conectMolecule ()
	{
		if (this.transform.parent.GetChild (1).position.x != this.transform.position.x) {
			// create SpringJoint to implement covalent bond between these two atoms
			spring = this.gameObject.AddComponent<SpringJoint> ();
			spring.connectedBody = this.transform.parent.GetChild (1).gameObject.GetComponent<Rigidbody> ();
			spring.anchor = new Vector3 (0, 0, 0);
			spring.connectedAnchor = new Vector3 (0, 0, 0);
			spring.spring = 100;
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
			spring.spring = 100;
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
