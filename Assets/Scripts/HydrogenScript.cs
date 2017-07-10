using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrogenScript : MonoBehaviour {
    //Physical of molecule
    private Rigidbody rb;
    //Controller every molecules
    private GameController gameController;
    // molar gas constant ( KJ/mol )
    private float R = 8.31447f * Mathf.Pow(10, -3);
    // temperature in kelvins (25+273)
    public float T = 298f;
    // mass of molecule argon
    private float massArgon = 10.00794f * Mathf.Pow(10, -3); // ( Kg / molecule )
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

    GameObject partnerOxygen = null;
    GameObject partnerHydrogen = null;
    SpringJoint springJoint;


    // Use this for initialization
    void Start () {
        gameController = GameController.getInstance();

        rb = GetComponent<Rigidbody>();

        alpha = Random.Range(-3.0f, 3.0f);
        beta = Random.Range(-3.0f, 3.0f);
        gamma = Random.Range(-3.0f, 3.0f);

        randomVector = new Vector3(alpha, beta, gamma);

        calculateValue = Mathf.Pow(alpha, 2) + Mathf.Pow(beta, 2) + Mathf.Pow(gamma, 2);

        scalar = Mathf.Sqrt(calculateValue);

        //unitVector = (1 / scalar) * randomVector;

        velocity = Mathf.Sqrt((3 * R * T) / (massArgon * calculateValue));

        velocityVector = velocity * randomVector;

        momentumVector = massArgon * velocityVector;

        rb.velocity = momentumVector;
    }
	
	// Update is called once per frame
	void Update () {
        periodicBoundary();
    }

    void OnTriggerEnter(Collider other)
    {
        if (partnerHydrogen == null && other.gameObject.CompareTag("Hydrogen"))
        {
            HydrogenScript otherHydrogen = (HydrogenScript)other.gameObject.GetComponent("HydrogenScript");

            if (otherHydrogen.partnerHydrogen == null) // two free radicals meet and form covalent bond
            {
                partnerHydrogen = other.gameObject;
                otherHydrogen.partnerHydrogen = this.gameObject;

                // chemical bond formation suddenly pulls slightly closer together
                float deltaX = partnerHydrogen.transform.position.x - this.transform.position.x;
                float deltaY = partnerHydrogen.transform.position.y - this.transform.position.y;
                float deltaZ = partnerHydrogen.transform.position.z - this.transform.position.z;
                this.transform.position = new Vector3(
                  this.transform.position.x + 0.25f * deltaX,
                  this.transform.position.y + 0.25f * deltaY,
                  this.transform.position.z + 0.25f * deltaZ);
                  partnerHydrogen.transform.position = new Vector3(
                  partnerHydrogen.transform.position.x - 0.25f * deltaX,
                  partnerHydrogen.transform.position.y - 0.25f * deltaY,
                  partnerHydrogen.transform.position.z - 0.25f * deltaZ);

                // create SpringJoint to implement covalent bond between these two atoms
                springJoint = this.gameObject.AddComponent<SpringJoint>();
                springJoint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
                springJoint.anchor = new Vector3(0, 0, 0);
                springJoint.connectedAnchor = new Vector3(0, 0, 0);
                springJoint.spring = 10;
                springJoint.minDistance = 0.0f;
                springJoint.maxDistance = 0.0f;
                springJoint.tolerance = 0.025f;
                springJoint.breakForce = Mathf.Infinity;
                springJoint.breakTorque = Mathf.Infinity;
                springJoint.enableCollision = false;
                springJoint.enablePreprocessing = true;
            }
        }

        if (partnerOxygen == null && other.gameObject.CompareTag("Oxygen"))
        {
            OxygenScript otherOxygen = (OxygenScript)other.gameObject.GetComponent("OxygenScript");

            if (otherOxygen.partnerHydrogen == null) // two free radicals meet and form covalent bond
            {
                partnerOxygen = other.gameObject;
                otherOxygen.partnerHydrogen = this.gameObject;

                // chemical bond formation suddenly pulls slightly closer together
                float deltaX = partnerHydrogen.transform.position.x - this.transform.position.x;
                float deltaY = partnerHydrogen.transform.position.y - this.transform.position.y;
                float deltaZ = partnerHydrogen.transform.position.z - this.transform.position.z;
                this.transform.position = new Vector3(
                  this.transform.position.x + 0.25f * deltaX,
                  this.transform.position.y + 0.25f * deltaY,
                  this.transform.position.z + 0.25f * deltaZ);
                partnerHydrogen.transform.position = new Vector3(
                partnerHydrogen.transform.position.x - 0.25f * deltaX,
                partnerHydrogen.transform.position.y - 0.25f * deltaY,
                partnerHydrogen.transform.position.z - 0.25f * deltaZ);

                // create SpringJoint to implement covalent bond between these two atoms
                springJoint = this.gameObject.AddComponent<SpringJoint>();
                springJoint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
                springJoint.anchor = new Vector3(0, 0, 0);
                springJoint.connectedAnchor = new Vector3(0, 0, 0);
                springJoint.spring = 10;
                springJoint.minDistance = 0.0f;
                springJoint.maxDistance = 0.0f;
                springJoint.tolerance = 0.025f;
                springJoint.breakForce = Mathf.Infinity;
                springJoint.breakTorque = Mathf.Infinity;
                springJoint.enableCollision = false;
                springJoint.enablePreprocessing = true;
            }
        }
    }

    //Periodic Boundary for set position of molecule ,when out side the box to opposite of the box
    void periodicBoundary()
    {
        Vector3 position = this.transform.position;
        if (position.x >= 5.01f)
        {
            position.x = -5.01f;
        }
        else if (position.x <= -5.01f)
        {
            position.x = 5.01f;
        }

        if (position.y >= 5.01f)
        {
            position.y = -5.01f;
        }
        else if (position.y <= -5.01f)
        {
            position.y = 5.01f;
        }

        if (position.z >= 5.01f)
        {
            position.z = -5.01f;
        }
        else if (position.z <= -5.01f)
        {
            position.z = 5.01f;
        }
        rb.MovePosition(position);
    }

}
