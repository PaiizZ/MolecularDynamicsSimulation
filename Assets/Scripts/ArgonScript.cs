using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgonScript : MonoBehaviour
{
	private Rigidbody rb;

	private GameController gameController;

	// molar gas constant ( KJ/mol )
	private float R = 8.31447f * Mathf.Pow (10, -3);
	// temperature in kelvins (25+273)
	public float T = 298f;
	// mass of molecule argon
	private float massArgon = 39.948f * Mathf.Pow (10, -3); // ( Kg / molecule )
	// attributes
	private float alpha;
	private float beta;
	private float gamma;
	private float calculateValue;
	private float scalar;
	private float velocity;
	private Vector3 unitVector;
	private Vector3 randomVector;
	private Vector3 velocityVector;
	public Vector3 momentumVector;
	private Vector3 forceVector;
	private float maxDistance = 2f;
	//attributes for set max velocity
	private float sqrMaxVelocity;
	// attributes for calculate force(Lennaed Jones potential)
	private Vector3 positionVector;
	private Vector3 tempObjectPosition;
	private Transform otherTransformObj;
	private Vector3[] forceFromObj;
	private int numberOfMolecule;
	private float time;
	// attributes for show value on sence
	public Vector3 objForce;
	public Vector3 objPosition;
	public string objName;
	// boolean to check, when mouse click molecule
	public bool clickOn;

	// Use this for initialization
	void Start ()
	{
		gameController = GameController.getInstance ();

		rb = GetComponent<Rigidbody> ();

		this.numberOfMolecule = gameController.getNumberArgon ();

		this.objForce = new Vector3 ();

		this.forceFromObj = new Vector3[this.numberOfMolecule + 1];

		this.clickOn = false;

		setMaxVelocity (8f);
      
		alpha = Random.Range (-3.0f, 3.0f);
		beta = Random.Range (-3.0f, 3.0f);
		gamma = Random.Range (-3.0f, 3.0f);

		randomVector = new Vector3 (alpha, beta, gamma);

		calculateValue = Mathf.Pow (alpha, 2) + Mathf.Pow (beta, 2) + Mathf.Pow (gamma, 2);
      
		scalar = Mathf.Sqrt (calculateValue);
        
		unitVector = (1 / scalar) * randomVector;
        
		velocity = Mathf.Sqrt ((3 * R * T) / (massArgon * calculateValue));
      
		velocityVector = velocity * randomVector;     

		momentumVector = massArgon * velocityVector;      

		rb.velocity = momentumVector;      
	}
		
	// Update is called once per frame
	void Update ()
	{
		setTempPosition ();
		this.time = Time.deltaTime * Mathf.Pow (10, -12);
		momentumVector = momentumVector + (0.5f * time * forceVector);
		calculationcForce (time);
		periodicBoundary ();
		if (rb.velocity.sqrMagnitude > this.sqrMaxVelocity) {
			Debug.Log (" rb.velocity.velocity1 " + rb.velocity.x + " y " + rb.velocity.y + " z " + rb.velocity.z);
			Debug.Log (" rb.velocity.normalized " + rb.velocity.normalized);
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			rb.velocity = massArgon * velocityVector;
			Debug.Log (" rb.velocity.velocity2 " + rb.velocity.x + " y " + rb.velocity.y + " z " + rb.velocity.z);
		}
		checkOnClick ();       
	}
		
	public void calculationcForce (float time)
	{
		float wellDepth = 0.128f; //constant well depth of argon (KJ/mol)
		float diameter = 3.42f;  //constant diameter of argon (Angstrom)  
       
		for (int i = 0; i < this.numberOfMolecule; i++) {
			this.otherTransformObj = gameController.transform.GetChild (i);
			Vector3 position = transform.position;
			Vector3 tempPosition = otherTransformObj.position;
			periodicBoundary ();
			if (position.x != tempPosition.x && position.y != tempPosition.y && position.z != tempPosition.z) {

				float distance = (Mathf.Sqrt (Mathf.Pow ((tempPosition.x - position.x), 2) + Mathf.Pow ((tempPosition.y - position.y), 2) + Mathf.Pow ((tempPosition.z - position.z), 2)));
				Vector3 temp = new Vector3 (distance, distance, distance) + (time * this.momentumVector / massArgon);
				float scalarDistance = (Mathf.Sqrt (Mathf.Pow ((temp.x), 2) + Mathf.Pow ((temp.y), 2) + Mathf.Pow ((temp.z), 2)));


				float distance2 = (Mathf.Sqrt (Mathf.Pow ((tempPosition.x - tempObjectPosition.x), 2) + Mathf.Pow ((tempPosition.y - tempObjectPosition.y), 2) + Mathf.Pow ((tempPosition.z - tempObjectPosition.z), 2)));
				Vector3 temp2 = new Vector3 (distance2, distance2, distance2) + (time * this.momentumVector / massArgon);
				float scalarDistance2 = (Mathf.Sqrt (Mathf.Pow ((temp2.x), 2) + Mathf.Pow ((temp2.y), 2) + Mathf.Pow ((temp2.z), 2)));

				if (scalarDistance <= maxDistance) {
					float energy = 12 * 4 * wellDepth * Mathf.Pow (diameter, 12) * Mathf.Pow (scalarDistance, -14) - 6 * 4 * wellDepth * Mathf.Pow (diameter, 6) * Mathf.Pow (scalarDistance, -8);
					Vector3 force = 0.5f * (-energy * (tempPosition - position) * time);
					rb.AddForce (force);
					this.delObjForce (forceFromObj [i]);
					forceFromObj [i] = force;
					this.addObjForce (forceFromObj [i]);
				} else if (scalarDistance2 <= maxDistance) {
					float energy = 12 * 4 * wellDepth * Mathf.Pow (diameter, 12) * Mathf.Pow (scalarDistance2, -14) - 6 * 4 * wellDepth * Mathf.Pow (diameter, 6) * Mathf.Pow (scalarDistance2, -8);
					Vector3 force = 0.5f * (-energy * (tempPosition - tempObjectPosition) * time);
					rb.AddForce (force);
					this.delObjForce (forceFromObj [i]);
					forceFromObj [i] = force;
					this.addObjForce (forceFromObj [i]);
				} else {
					float energy = 12 * 4 * wellDepth * Mathf.Pow (diameter, 12) * Mathf.Pow (scalarDistance, -14) - 6 * 4 * wellDepth * Mathf.Pow (diameter, 6) * Mathf.Pow (scalarDistance, -8);             
					Vector3 force = 0.5f * (energy * (tempPosition - position) * time);
					rb.AddForce (force);
					this.delObjForce (forceFromObj [i]);
					forceFromObj [i] = force;
					this.addObjForce (forceFromObj [i]);
				}
			} else {
				this.objName = "Argon " + i;
				objPosition = position;
			}
		}
	}

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
	}

	void setTempPosition ()
	{
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

	void setMaxVelocity (float maxVelocity)
	{
		this.sqrMaxVelocity = Mathf.Pow (maxVelocity, 2);
	}

	void delObjForce (Vector3 force)
	{
		this.objForce -= force;
	}

	void addObjForce (Vector3 force)
	{
		this.objForce += force;
	}

	public void OnMouseDown ()
	{
		this.clickOn = true;
		GameController.getInstance ().changeArgonFocus (this);
	}

	public void changeOnClick ()
	{
		this.clickOn = false;
	}

	void checkOnClick ()
	{
		if (clickOn) {
			this.GetComponent<Renderer> ().material.color = Color.green;
		} else if (!clickOn) {
			this.GetComponent<Renderer> ().material.color = Color.red;
		}
	}

}