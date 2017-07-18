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
	private HydrogenScript partnerHydrogen1;
	private HydrogenScript partnerHydrogen2;
	private float lengthO_H = .1f;
	private float lengthH_H = .1633f;


	SpringJoint springJoint1;


	// public List<HydrogenScript> hydrogens = new List<HydrogenScript>();

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



		// initialization partner of oxygen
		position = this.transform.position;
        //hydrogens.Add(Instantiate(partnerHydrogen1, new Vector3(position.x - lengthH_H / 2, position.y - Mathf.Sqrt(Mathf.Pow(lengthO_H, 2) - Mathf.Pow(lengthH_H / 2, 2)), this.transform.position.z), Quaternion.identity));
        //hydrogens.Add(Instantiate(partnerHydrogen2, new Vector3(position.x + lengthH_H / 2, position.y - Mathf.Sqrt(Mathf.Pow(lengthO_H, 2) - Mathf.Pow(lengthH_H / 2, 2)), this.transform.position.z), Quaternion.identity));

        //foreach (HydrogenScript hydrogen in hydrogens){
        //    hydrogen.transform.SetParent(this.transform);
        //}
       
		//conectMolecule ();
		//rb.velocity = momentumVector;
	}

	// Update is called once per frame
	void Update ()
	{
		//periodicBoundary();
	}

	void conectMolecule ()
	{

		// chemical bond formation suddenly pulls slightly closer together
		float deltaX1 = partnerHydrogen1.transform.position.x - this.transform.position.x;
		float deltaY1 = partnerHydrogen1.transform.position.y - this.transform.position.y;
		float deltaZ1 = partnerHydrogen1.transform.position.z - this.transform.position.z;
		this.transform.position = new Vector3 (
			this.transform.position.x + 0.25f * deltaX1,
			this.transform.position.y + 0.25f * deltaY1,
			this.transform.position.z + 0.25f * deltaZ1);
		partnerHydrogen1.transform.position = new Vector3 (
			partnerHydrogen1.transform.position.x - 0.25f * deltaX1,
			partnerHydrogen1.transform.position.y - 0.25f * deltaY1,
			partnerHydrogen1.transform.position.z - 0.25f * deltaZ1);

		// create SpringJoint to implement covalent bond between these two atoms
		springJoint1 = this.gameObject.AddComponent<SpringJoint> ();
		springJoint1.connectedBody = partnerHydrogen1.gameObject.GetComponent<Rigidbody> ();
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

       

	}

	//Periodic Boundary for set position of molecule ,when out side the box to opposite of the box
	void periodicBoundary ()
	{
		this.position = this.transform.position;
		Vector3 positionH1 = partnerHydrogen1.transform.position;
		Vector3 positionH2 = partnerHydrogen2.transform.position;
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