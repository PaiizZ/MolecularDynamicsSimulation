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
	private float massArgon = 15.9994f * Mathf.Pow (10, -3);
	// ( Kg / molecule )
	// attributes
	private float alpha;
	private float beta;
	private float gamma;
	private float calculateValue;
	private float scalar;
	private float velocity;
	private Vector3 position;
	private Vector3 randomVector;
	private Vector3 velocityVector;
	public Vector3 momentumVector;
	//arttributes of partner
	private Vector3 partnerHydrogen1;
	private Vector3 partnerHydrogen2;
	private float lengthO_H = .1f;
	private float lengthH_H = .1633f;

	SpringJoint springJoint1;
	SpringJoint springJoint2;

	public HydrogenScript hydrogenPerfab;
	public List<HydrogenScript> hydrogens = new List<HydrogenScript>();

	
	//spring force
	Vector3 posO;
	Vector3 posH1;
	Vector3 posH2;
	Vector3 posOH1;
	Vector3 posOH2;

	float enegy;
//	public static GameController getInstance ()
//	{
//		return GameObject.Find ("GameController").GetComponent<GameController> ();
//	}
	
	// Use this for initialization
	void Start ()
	{
		gameController = GameController.getInstance ();

		rb = GetComponent<Rigidbody> ();

		position = this.transform.position;

		// initialization partner of oxygen
		initialMolecule ();

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
		
		conectHydrogenMolecule ();
		rb.velocity = momentumVector;
		//Debug.Log ("O  "+momentumVector.x + " " + momentumVector.y + " " + momentumVector.z);
	}

	void initialMolecule ()
	{
		hydrogens.Add (Instantiate (hydrogenPerfab, new Vector3 (position.x - 0.1633f / 2f, position.y - Mathf.Sqrt (0.1f * 0.1f - Mathf.Pow (0.1633f / 2f, 2f)), position.z), Quaternion.identity));
		hydrogens.Add (Instantiate (hydrogenPerfab, new Vector3 (position.x + 0.1633f / 2f, position.y - Mathf.Sqrt (0.1f * 0.1f - Mathf.Pow (0.1633f / 2f, 2f)), position.z), Quaternion.identity));
		foreach (HydrogenScript h in hydrogens) {
			h.transform.SetParent (this.transform);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		periodicBoundary();
		//this.transform.Translate (momentumVector*Time.deltaTime);
	}

	void springForce (){
		this.posO = this.position;
		this.posH1 = this.transform.GetChild(0).position;
		this.posH2 = this.transform.GetChild(1).position;
		this.posOH1 = posH1 - posO;
		this.posOH2 = posH2 - posO;
		float angle0 = 1.9106f ;// (rad)
		float K0 = 383f * Mathf.Pow(10,-2) ; // (KJ/mol/rad^2)
		float scalarOH1 = Mathf.Sqrt(Mathf.Pow(posH1.x - posO.x,2)+Mathf.Pow(posH1.y - posO.y,2)+Mathf.Pow(posH1.z - posO.z,2));
		//Debug.Log ("scalarOH1 " + scalarOH1);
		float scalarOH2 = Mathf.Sqrt(Mathf.Pow(posH2.x - posO.x,2)+Mathf.Pow(posH2.y - posO.y,2)+Mathf.Pow(posH2.z - posO.z,2));
		//Debug.Log ("scalarOH2 " + scalarOH2);
		float OH1OH2 = (posH1.x-posO.x)*(posH2.x-posO.x)+(posH1.y-posO.y)*(posH2.y-posO.y)+(posH1.z-posO.z)*(posH2.z-posO.z); // (posH1 * posOH2)
		//Debug.Log ("OH1OH2 " + OH1OH2);
		float angle = Mathf.Acos(OH1OH2/(scalarOH1*scalarOH2)) ;// (rad)
//		Debug.Log ("angle " + angle);
		this.enegy = K0 * (angle-angle0);
//		Debug.Log ("enegy " + enegy);
//		Debug.Log ("Mathf.sin(angle) : " + 1/Mathf.Sin(angle));
//		Debug.Log ("Mathf.Asin(angle) : " + Mathf.Asin(angle));
//		Debug.Log ("1/(scalarOH1*scalarOH2) : " + 1/(scalarOH1*scalarOH2));
//		Debug.Log (": " + (posOH2 - posOH1*((scalarOH1*scalarOH2)/Mathf.Pow(scalarOH1,2))));
		Vector3 forceH1 =  enegy/Mathf.Sin(angle) * 1/(scalarOH1*scalarOH2) * (posOH2 - posOH1*((scalarOH1*scalarOH2)/Mathf.Pow(scalarOH1,2)));
		Debug.Log ("force1 x:" + forceH1.x + " y:" + forceH1.y + " z:" + forceH1.z);
		Vector3 forceH2 =  enegy/Mathf.Sin(angle) * 1/(scalarOH1*scalarOH2) * (posOH1 - posOH2*((scalarOH1*scalarOH2)/Mathf.Pow(scalarOH2,2)));
//		Debug.Log ("force2 " + forceOH2);
		Vector3 forceO = - forceH1 - forceH2;
		rb.AddForce(forceO);
		this.transform.GetChild (0).gameObject.GetComponent<Rigidbody> ().AddForce (forceH1);
		this.transform.GetChild (1).gameObject.GetComponent<Rigidbody> ().AddForce (forceH2);
	}
	
	void conectHydrogenMolecule ()
	{
		// create SpringJoint to implement covalent bond between these two atoms
		springJoint1 = this.gameObject.AddComponent<SpringJoint> ();
		springJoint1.connectedBody = this.transform.GetChild (0).gameObject.GetComponent<Rigidbody> ();
		springJoint1.anchor = new Vector3 (0, 0, 0);
		springJoint1.connectedAnchor = new Vector3 (0, 0, 0);
		springJoint1.spring = 10;
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
		springJoint2.spring = 10;
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
		Vector3 positionH1 = this.transform.GetChild(0).position;
		Vector3 positionH2 = this.transform.GetChild(1).position;
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
}