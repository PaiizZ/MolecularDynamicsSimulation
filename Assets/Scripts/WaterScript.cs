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

	}

	void springForce ()
	{
		Vector3 hydrogen1 = this.transform.GetChild (1).position;
		Vector3 hydrogen2 = this.transform.GetChild (2).position;
		// chemical bond formation suddenly pulls slightly closer together
		float deltaX1 = hydrogen2.x - hydrogen1.x;
		float deltaY1 = hydrogen2.y - hydrogen1.y;
		float deltaZ1 = hydrogen2.z - hydrogen1.z;
		this.transform.position = new Vector3 (
			this.transform.position.x + 0.25f * deltaX1,
			this.transform.position.y + 0.25f * deltaY1,
			this.transform.position.z + 0.25f * deltaZ1);
		hydrogen1 = new Vector3 (
			hydrogen1.x - 0.25f * deltaX1,
			hydrogen1.y - 0.25f * deltaY1,
			hydrogen1.z - 0.25f * deltaZ1);

		// create SpringJoint to implement covalent bond between these two atoms
		spring = this.gameObject.AddComponent<SpringJoint> ();
		spring.connectedBody = this.transform.GetChild (1).gameObject.GetComponent<Rigidbody> ();
		spring.anchor = new Vector3 (0, 0, 0);
		spring.connectedAnchor = new Vector3 (0, 0, 0);
		spring.spring = 10;
		spring.minDistance = 0.0f;
		spring.maxDistance = 0.0f;
		spring.tolerance = 0.025f;
		spring.breakForce = Mathf.Infinity;
		spring.breakTorque = Mathf.Infinity;
		spring.enableCollision = false;
		spring.enablePreprocessing = true;

	}

}
