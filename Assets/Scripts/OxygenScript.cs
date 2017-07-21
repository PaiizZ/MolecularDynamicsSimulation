using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenScript : MonoBehaviour
{
	//Physical of molecule
	private Rigidbody rb;
	//Controller every molecules
	private GameController gameController;
	// molar gas constant ( KJ/mol )
	private float R = 8.31447f * Mathf.Pow (10, -3);
	// temperature in kelvins (25+273)
	public float T = 298f;
	// mass of molecule argon
	private float massArgon = 15.9994f * Mathf.Pow (10, -3); // ( Kg / molecule )

	// attributes
	private float alpha;
	private float beta;
	private float gamma;
	private float calculateValue;
	private float velocity;
	private float time;
	private Vector3 position;
	private Vector3 randomVector;
	private Vector3 velocityVector;
	public Vector3 momentumVector;
	private Vector3 forceVector;

	// attributes for calculate force(Lennaed Jones potential)
	private Vector3 tempObjectPosition;
	private Transform otherTransformObj;
	private Vector3[] forceFromObj;
	private int numberOfWater;
	private float wellDepth = 0.118f; //constant well depth of argon (KJ/mol)
	private float diameter = 3.58f; //constant diameter of argon (Angstrom)
	private float minDistance = 3f;

	// attributes for show value on sence
	public Vector3 objForce;
	public Vector3 objPosition;
	public string objName;
	
	//arttributes of partner
	private Vector3 partnerHydrogen1;
	private Vector3 partnerHydrogen2;

	//spring force
	Vector3 posO;
	Vector3 posH1;
	Vector3 posH2;
	Vector3 posOH1;
	Vector3 posOH2;
	public Vector3 forceH1;
	public Vector3 forceH2;
	Vector3 forceO;
	float enegy;
	SpringJoint springJoint1;
	SpringJoint springJoint2;

	public HydrogenScript hydrogenPerfab;
	public List<HydrogenScript> hydrogens = new List<HydrogenScript> ();

	
	// Use this for initialization
	void Start ()
	{
		gameController = GameController.getInstance ();

		rb = GetComponent<Rigidbody> ();

		position = this.transform.position;

		this.numberOfWater = gameController.getNumberOxygen ();

		this.objForce = new Vector3 ();

		this.forceFromObj = new Vector3[this.numberOfWater + 1];

		initialMolecule ();

		alpha = Random.Range (-3.0f, 3.0f);
		beta = Random.Range (-3.0f, 3.0f);
		gamma = Random.Range (-3.0f, 3.0f);

		randomVector = new Vector3 (alpha, beta, gamma);

		calculateValue = Mathf.Pow (alpha, 2) + Mathf.Pow (beta, 2) + Mathf.Pow (gamma, 2);

		velocity = Mathf.Sqrt ((3 * R * T) / (massArgon * calculateValue));

		velocityVector = velocity * randomVector;

		momentumVector = massArgon * velocityVector;
		
		conectHydrogenMolecule ();
	}
		
	// Update is called once per frame
	void Update ()
	{
		this.time = Time.deltaTime * Mathf.Pow (10, -12);
		momentumVector = momentumVector + (0.5f * time * forceVector);
		springForce ();
		forceVector = forceO;
		momentumVector = momentumVector + (0.5f * time * forceVector);
		rb.velocity = momentumVector;
		vdwEquation ();
		electrostatic ();
		periodicBoundary ();

	}

	// initialization partner of oxygen
	void initialMolecule ()
	{

		float lengthO_H = 0.1f;
		float lengthH_H = 0.1633f;
		Vector3 posO = this.position;
		float posxH1 = position.x - lengthH_H / 2f;
		float posyH1 = position.y - Mathf.Sqrt (Mathf.Pow (lengthO_H, 2) - Mathf.Pow (lengthH_H / 2, 2));
		float poszH1 = position.z;
		float posxH2 = position.x + lengthH_H / 2f;
		float posyH2 = position.y - Mathf.Sqrt (Mathf.Pow (lengthO_H, 2) - Mathf.Pow (lengthH_H / 2, 2));
		float poszH2 = position.z;

		hydrogens.Add (Instantiate (hydrogenPerfab, new Vector3 (posxH1, posyH1, poszH2), Quaternion.identity));
		hydrogens.Add (Instantiate (hydrogenPerfab, new Vector3 (posxH2, posyH2, poszH2), Quaternion.identity));

		foreach (HydrogenScript h in hydrogens) {
			h.transform.SetParent (this.transform);
		}
	}

	//calculate force by van-der-wan equation 
	public void vdwEquation ()
	{
		for (int i = 0; i < this.numberOfWater; i++) {
			this.otherTransformObj = gameController.transform.GetChild (i);
			Vector3 position = transform.position;
			Vector3 tempPosition = otherTransformObj.position;
			periodicBoundary ();
			if (position.x != tempPosition.x && position.y != tempPosition.y && position.z != tempPosition.z) {

				float distance = (Mathf.Sqrt (Mathf.Pow ((tempPosition.x - position.x), 2) + Mathf.Pow ((tempPosition.y - position.y), 2) + Mathf.Pow ((tempPosition.z - position.z), 2)));
			
				float distance2 = (Mathf.Sqrt (Mathf.Pow ((tempPosition.x - tempObjectPosition.x), 2) + Mathf.Pow ((tempPosition.y - tempObjectPosition.y), 2) + Mathf.Pow ((tempPosition.z - tempObjectPosition.z), 2)));

				if (distance <= minDistance) {
					float energy = 12 * 4 * wellDepth * Mathf.Pow (diameter, 12) * Mathf.Pow (distance, -14) - 6 * 4 * wellDepth * Mathf.Pow (diameter, 6) * Mathf.Pow (distance, -8);
					Vector3 force = 0.5f * (-energy * (tempPosition - position));
					rb.AddForce (force);
					this.delObjForce (forceFromObj [i]);
					forceFromObj [i] = force;
					this.addObjForce (forceFromObj [i]);
					forceVector += force;
				} else if (distance2 <= minDistance) {
					float energy = 12 * 4 * wellDepth * Mathf.Pow (diameter, 12) * Mathf.Pow (distance2, -14) - 6 * 4 * wellDepth * Mathf.Pow (diameter, 6) * Mathf.Pow (distance2, -8);
					Vector3 force = 0.5f * (-energy * (tempPosition - tempObjectPosition));
					rb.AddForce (force);
					this.delObjForce (forceFromObj [i]);
					forceFromObj [i] = force;
					this.addObjForce (forceFromObj [i]);
					forceVector += force;
				} else {
					float energy = 12 * 4 * wellDepth * Mathf.Pow (diameter, 12) * Mathf.Pow (distance, -14) - 6 * 4 * wellDepth * Mathf.Pow (diameter, 6) * Mathf.Pow (distance, -8);             
					Vector3 force = 0.5f * (energy * (tempPosition - position));
					rb.AddForce (force);
					this.delObjForce (forceFromObj [i]);
					forceFromObj [i] = force;
					this.addObjForce (forceFromObj [i]);
					forceVector += force;
				}
			} else {
				this.objName = "Water " + i;
				objPosition = position;
			}
		}
	}

	// calculate force in spring between atom in molecule
	void springForce ()
	{
		this.posO = this.position;
		this.posH1 = this.transform.GetChild (0).position;
		this.posH2 = this.transform.GetChild (1).position;
		this.posOH1 = posH1 - posO;
		this.posOH2 = posH2 - posO;
		float angle0 = 1.9106f;// (rad)
		float K0 = 383f * Mathf.Pow (10, -3); // (KJ/mol/rad^2)
		float scalarOH1 = Mathf.Sqrt (Mathf.Pow (posH1.x - posO.x, 2) + Mathf.Pow (posH1.y - posO.y, 2) + Mathf.Pow (posH1.z - posO.z, 2));
		float scalarOH2 = Mathf.Sqrt (Mathf.Pow (posH2.x - posO.x, 2) + Mathf.Pow (posH2.y - posO.y, 2) + Mathf.Pow (posH2.z - posO.z, 2));
		float OH1OH2 = (posH1.x - posO.x) * (posH2.x - posO.x) + (posH1.y - posO.y) * (posH2.y - posO.y) + (posH1.z - posO.z) * (posH2.z - posO.z); 
		float angle = Mathf.Acos (OH1OH2 / (scalarOH1 * scalarOH2));// (rad)
		this.enegy = K0 * (angle - angle0);

		forceH1 = enegy / Mathf.Sin (angle) * 1 / (scalarOH1 * scalarOH2) * (posOH2 - posOH1 * ((scalarOH1 * scalarOH2) / Mathf.Pow (scalarOH1, 2)));
		forceH2 = enegy / Mathf.Sin (angle) * 1 / (scalarOH1 * scalarOH2) * (posOH1 - posOH2 * ((scalarOH1 * scalarOH2) / Mathf.Pow (scalarOH2, 2)));
		forceO = -forceH1 - forceH2;

		rb.AddForce (forceO);
		this.transform.GetChild (0).gameObject.GetComponent<Rigidbody> ().AddForce (forceH1);
		this.transform.GetChild (1).gameObject.GetComponent<Rigidbody> ().AddForce (forceH2);
	}

	//calculate an electrostatic force every pair
	void electrostatic ()
	{
		float Kspring = 9 * Mathf.Pow (10, 15); // (KJnm/c^2)
		float elementaryCharge = 1.602f * Mathf.Pow (10, -19);// c
		float electricChargeOxygen = -0.82f * elementaryCharge; // c
		float electricChargeHydrogen = 0.41f * elementaryCharge; // c
		float numberHydrogeninWater = 2;
		List<Vector3> posAtoms = new List<Vector3> ();
		Vector3 position = this.transform.position; 
		for (int i = 0; i < numberOfWater; i++) {
			Transform transformChild = this.transform.parent.GetChild (i);

			if (position != transformChild.position) {
				posAtoms.Add (transformChild.position);
			}
			for (int j = 0; j < numberHydrogeninWater; j++) {
				if (position != transformChild.GetChild (j).position) {
					posAtoms.Add (transformChild.GetChild (j).position);
				}
			}
		}

		foreach (Vector3 tempPos in posAtoms) {
			Vector3 unitVector = tempPos - position;
			Vector3 springForce = ((Kspring * electricChargeOxygen * electricChargeHydrogen)
			                      / Mathf.Pow (Mathf.Pow (tempPos.x - position.x, 2) + Mathf.Pow (tempPos.y - position.y, 2) + Mathf.Pow (tempPos.z - position.z, 2), 1.5f)) * unitVector;
			rb.AddForce (springForce);
		}
	}

	// combine between hydrogen atom
	void conectHydrogenMolecule ()
	{
		// create SpringJoint to implement covalent bond between these two atoms
		springJoint1 = this.gameObject.AddComponent<SpringJoint> ();
		springJoint1.connectedBody = this.transform.GetChild (0).gameObject.GetComponent<Rigidbody> ();
		springJoint1.anchor = new Vector3 (0, 0, 0);
		springJoint1.connectedAnchor = new Vector3 (0, 0, 0);
		springJoint1.spring = 34500;
		springJoint1.minDistance = 0.0f;
		springJoint1.maxDistance = 0.0f;
		springJoint1.tolerance = 0.025f;
		springJoint1.breakForce = Mathf.Infinity;
		springJoint1.breakTorque = Mathf.Infinity;
		springJoint1.enableCollision = false;
		springJoint1.enablePreprocessing = true;

		// create SpringJoint to implement covalent bond between these two atoms
		springJoint2 = this.gameObject.AddComponent<SpringJoint> ();
		springJoint2.connectedBody = this.transform.GetChild (1).gameObject.GetComponent<Rigidbody> ();
		springJoint2.anchor = new Vector3 (0, 0, 0);
		springJoint2.connectedAnchor = new Vector3 (0, 0, 0);
		springJoint2.spring = 34500;
		springJoint2.minDistance = 0.0f;
		springJoint2.maxDistance = 0.0f;
		springJoint2.tolerance = 0.025f;
		springJoint2.breakForce = Mathf.Infinity;
		springJoint2.breakTorque = Mathf.Infinity;
		springJoint2.enableCollision = false;
		springJoint2.enablePreprocessing = true;

	}


	//Periodic Boundary for set position of molecule ,when out side the box to opposite of the box
	void periodicBoundary ()
	{
		this.position = this.transform.position;
		Vector3 positionH1 = this.transform.GetChild (0).position;
		Vector3 positionH2 = this.transform.GetChild (1).position;
		if (position.x >= 5.05f && positionH1.x >= 5.05f && positionH2.x >= 5.05f) {
			position.x = -5.05f;
		} else if (position.x <= -5.05f && positionH1.x <= -5.05f && positionH2.x <= -5.05f) {
			position.x = 5.05f;
		}

		if (position.y >= 5.05f && positionH1.y >= 5.05f && positionH2.y >= 5.05f) {
			position.y = -5.05f;
		} else if (position.y <= -5.05f && positionH1.y <= -5.05f && positionH2.y <= -5.05f) {
			position.y = 5.05f;
		}

		if (position.z >= 5.05f && positionH1.z >= 5.05f && positionH2.z >= 5.05f) {
			position.z = -5.05f;
		} else if (position.z <= -5.05f && positionH1.z <= -5.05f && positionH2.z <= -5.05f) {
			position.z = 5.05f;
		}

		this.transform.position = new Vector3 (position.x, position.y, position.z);
	}

	//Delete force from object molecule
	void delObjForce (Vector3 force)
	{
		this.objForce -= force;
	}

	//Add force to object molecule
	void addObjForce (Vector3 force)
	{
		this.objForce += force;
	}

	//Set position to temp object
	void setTempPosition ()
	{
		//Set tmep object position equals this object position 
		this.tempObjectPosition = this.transform.position;
		if (this.tempObjectPosition.x == Mathf.Abs (this.tempObjectPosition.x)) {
			this.tempObjectPosition.x -= 10;
		} else {
			this.tempObjectPosition.x += 10;
		}

		if (this.tempObjectPosition.y == Mathf.Abs (this.tempObjectPosition.y)) {
			this.tempObjectPosition.y -= 10;
		} else {
			this.tempObjectPosition.y += 10;
		}

		if (this.tempObjectPosition.z == Mathf.Abs (this.tempObjectPosition.z)) {
			this.tempObjectPosition.z -= 10;
		} else {
			this.tempObjectPosition.z += 10;
		}
	}
}