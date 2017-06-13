using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgonScripts : MonoBehaviour {
	private Rigidbody rb;
	private float R = 8.31447f ; //molar gas constant ( J/(mol*Kg) )
	private int T = 298 ; // temperature in kelvins (25+273)
	private float massArgon = 39.948f * Mathf.Pow(10,-3) ; // ( Kg/mol )
	private float alpha;
	private float beta;
	private float gamma;
	private double scaleVector;
	private double unitVector;
	private double velocity;
	private double momentum;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		alpha = Random.Range (-10.0f, 10.0f);
		beta = Random.Range (-10.0f, 10.0f);
		gamma = Random.Range (-10.0f, 10.0f);
		scaleVector = Mathf.Sqrt ( Mathf.Pow(alpha ,2) + Mathf.Pow(beta ,2) + Mathf.Pow(gamma ,2) );
		unitVector = 1 / (Mathf.Sqrt (Mathf.Pow (alpha, 2) + Mathf.Pow (beta, 2) + Mathf.Pow (gamma, 2))) * scaleVector;
		velocity = Mathf.Sqrt ( 3 * R * T / massArgon ) * unitVector ;

		Debug.Log (velocity);
	}

	// Update is called once per frame
	void Update () {

	}
}