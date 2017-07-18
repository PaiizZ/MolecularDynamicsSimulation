using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
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
	private float massWater = 18.01528f * Mathf.Pow (10, -3);
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

	public OxygenScript oxygenPerfab;
	public HydrogenScript hydrogenPerfab;

	public List<OxygenScript> moleculeO = new List<OxygenScript> ();
	public List<HydrogenScript> moleculeH = new List<HydrogenScript> ();
	SpringJoint spring;

	//spring force
	Vector3 posO;
	Vector3 posH1;
	Vector3 posH2;
	Vector3 posOH1;
	Vector3 posOH2;

	float enegy;


	// Use this for initialization
	void Start ()
	{
		gameController = GameController.getInstance ();

		rb = GetComponent<Rigidbody> ();

		position = this.transform.position;
		alpha = Random.Range (-3.0f, 3.0f);
		beta = Random.Range (-3.0f, 3.0f);
		gamma = Random.Range (-3.0f, 3.0f);

		randomVector = new Vector3 (alpha, beta, gamma);

		calculateValue = Mathf.Pow (alpha, 2) + Mathf.Pow (beta, 2) + Mathf.Pow (gamma, 2);

		scalar = Mathf.Sqrt (calculateValue);

		//unitVector = (1 / scalar) * randomVector;

		velocity = Mathf.Sqrt ((3 * R * T) / (massWater * calculateValue));

		velocityVector = velocity * randomVector;

		momentumVector = massWater * velocityVector;

		initialMolecule ();
		setParentMolecules ();

		rb.velocity = momentumVector;
		//rb.velocity = new Vector3(5f,5f,5f);
		//Debug.Log (this.transform.GetChild (0).position.x + " " + this.transform.GetChild (0).position.y + " " + this.transform.GetChild (0).position.z + " ");
		//Debug.Log (this.transform.GetChild (1).position.x + " " + this.transform.GetChild (1).position.y + " " + this.transform.GetChild (0).position.z + " ");
		//Debug.Log (this.transform.GetChild (2).position.x + " " + this.transform.GetChild (2).position.y + " " + this.transform.GetChild (0).position.z + " ");
	}

	void initialMolecule ()
	{
		moleculeO.Add (Instantiate (oxygenPerfab, new Vector3 (position.x, position.y, position.z), Quaternion.identity));
		moleculeH.Add (Instantiate (hydrogenPerfab, new Vector3 (position.x - 0.1633f / 2f, position.y - Mathf.Sqrt (0.1f * 0.1f - Mathf.Pow (0.1633f / 2f, 2f)), position.z), Quaternion.identity));
		moleculeH.Add (Instantiate (hydrogenPerfab, new Vector3 (position.x + 0.1633f / 2f, position.y - Mathf.Sqrt (0.1f * 0.1f - Mathf.Pow (0.1633f / 2f, 2f)), position.z), Quaternion.identity));
	}

	void setParentMolecules ()
	{
		foreach (OxygenScript o in moleculeO) {
			o.transform.SetParent (this.transform);
		}
		foreach (HydrogenScript h in moleculeH) {
			h.transform.SetParent (this.transform);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		//periodicBoundary ();
		springForce ();
	}

	void springForce (){
		this.posO = this.transform.GetChild(0).position;
		this.posH1 = this.transform.GetChild(1).position;
		this.posH2 = this.transform.GetChild(2).position;
		this.posOH1 = posH1 - posO;
		this.posOH2 = posH2 - posO;
		float angle0 = 1.9106f ;// (rad) 
		float K0 = 383 ; // (KJ/mol/rad^2)
		float scalarOH1 = Mathf.Sqrt(Mathf.Pow(posH1.x - posO.x,2)+Mathf.Pow(posH1.y - posO.y,2)+Mathf.Pow(posH1.z - posO.z,2));
		float scalarOH2 = Mathf.Sqrt(Mathf.Pow(posH2.x - posO.x,2)+Mathf.Pow(posH2.y - posO.y,2)+Mathf.Pow(posH2.z - posO.z,2));
		float OH1OH2 = (posH1.x-posO.x)*(posH2.x-posO.x)+(posH1.y-posO.y)*(posH2.y-posO.y)+(posH1.z-posO.z)*(posH2.z-posO.z); // (posH1 * posOH2)
		float angle = Mathf.Acos(OH1OH2/(scalarOH1*scalarOH2)) ;// (rad) 
		this.enegy = K0 * Mathf.Pow(angle-angle0,2);
		Vector3 forceOH1 = enegy * 1/(scalarOH1*scalarOH2) * (posOH2 - posOH1*((scalarOH1*scalarOH2)/Mathf.Pow(scalarOH1,2)));
		Vector3 forceOH2 = enegy * 1/(scalarOH1*scalarOH2) * (posOH1 - posOH2*((scalarOH1*scalarOH2)/Mathf.Pow(scalarOH2,2)));
		this.transform.GetChild (1).gameObject.GetComponent<Rigidbody> ().AddForce (forceOH1);
		this.transform.GetChild (2).gameObject.GetComponent<Rigidbody> ().AddForce (forceOH2);
	}

	//Periodic Boundary for set position of molecule ,when out side the box to opposite of the box
//	void periodicBoundary ()
//	{
//		this.posO = this.transform.GetChild(0).position;
//		this.posH1 = this.transform.GetChild(1).position;
//		this.posH2 = this.transform.GetChild(2).position;
//		if (posO.x >= 5.05f && posH1.x >= 5.05f && posH2.x >= 5.05f) {
//			position.x = -5.05f;
//		} else if (posO.x <= -5.05f && posH1.x <= -5.05f && posH2.x <= -5.05f) {
//			position.x = 5.05f;
//		}
//	
//		if (posO.y >= 5.05f && posH1.y >= 5.05f && posH2.y >= 5.05f) {
//			position.y = -5.05f;
//		} else if (posO.y <= -5.05f && posH1.y <= -5.05f && posH2.y <= -5.05f) {
//			position.y = 5.05f;
//		}
//	
//		if (posO.z >= 5.05f && posH1.z >= 5.05f && posH2.z >= 5.05f) {
//			position.z = -5.05f;
//		} else if (posO.z <= -5.05f && posH1.z <= -5.05f && posH2.z <= -5.05f) {
//			position.z = 5.05f;
//		}
//		Debug.Log ("x :" + this.position.x);
//		//this.transform.position = new Vector3 (position.x, position.y, position.z);
//		rb.MovePosition (position);
//	}

	void periodicBoundary ()
	{
		Vector3 position = this.transform.position;
		if (position.x >= 5.01f) {
			position.x = -5.01f;
		} else if (position.x <= -5.01f) {
			position.x = 5.01f;
		}

		if (position.y >= 5.01f) {
			position.y = -5.01f;
		} else if (position.y <= -5.01f) {
			position.y = 5.01f;
		}

		if (position.z >= 5.01f) {
			position.z = -5.01f;
		} else if (position.z <= -5.01f) {
			position.z = 5.01f;
		}
		rb.MovePosition (position);
		//this.transform.position = new Vector3 (position.x, position.y, position.z);
	}
}
