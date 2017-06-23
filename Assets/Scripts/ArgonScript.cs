using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgonScript : MonoBehaviour {
	private Rigidbody rb;
	private float R = 8.31447f ; //molar gas constant ( J/(mol*Kg) )
	private float T = 298f ; // temperature in kelvins (25+273)
	private float massArgon = 39.948f * Mathf.Pow(10,-3) ; // ( Kg/mol )
	private float alpha;
	private float beta;
	private float gamma;
	private float calculateValue;
	private float scalar;
	private float unitVector;
	private Vector3 velocityVector;
	private Vector3 momentumVector;
	private Vector3 forceVector;
	private Vector3 positionVector;
	private GameObject go;
	private GameController gameController;
	List<Vector3> listMomentum = new List<Vector3>();

	// Use this for initialization
	void Start () {
		
		GameObject go = GameObject.Find("GameController");
		gameController = (GameController) go.GetComponent(typeof(GameController));

		rb = GetComponent<Rigidbody>();
//		Debug.Log (4*128*Mathf.Pow(342,12)+" eiei " + 4*128*Mathf.Pow(342,6));
		alpha = Random.Range (-10.0f, 10.0f);
		beta = Random.Range (-10.0f, 10.0f);
		gamma = Random.Range (-10.0f, 10.0f);

		calculateValue = Mathf.Pow (alpha, 2) + Mathf.Pow (beta, 2) + Mathf.Pow (gamma, 2);
//		Debug.Log ("calculateValue: " + calculateValue);

		scalar = Mathf.Sqrt (calculateValue);
//		Debug.Log ("scaleVector: " + scalar);

		unitVector = 1.0f / (scalar) * scalar;
//		Debug.Log ("unitVector: " + unitVector);

		velocityVector = new Vector3(alpha * Mathf.Sqrt ((3 * R * T) / (massArgon*calculateValue)) , beta * Mathf.Sqrt ((3 * R * T) / (massArgon*calculateValue)), gamma * Mathf.Sqrt ((3 * R * T) / (massArgon*calculateValue)));
//		Debug.Log ("velocity x:" + velocityVector.x +" y:" + velocityVector.y + " z:"+velocityVector.z);

		momentumVector = massArgon * velocityVector;
//		Debug.Log ("momentum x:" + momentumVector.x +" y:" + momentumVector.y + " z:"+momentumVector.z);

//		rb.velocity = momentum ;
//		for(int i = 0 ; i < gameController.getNumberArgon() ; i++){
//			if(this.transform.position.x != gameController.gameObject.transform.GetChild(i).position.x){
//			Debug.Log ("x:" + gameController.gameObject.transform.GetChild(i).position.x+" y:" +  gameController.gameObject.transform.GetChild(i).position.y+" z:" +  gameController.gameObject.transform.GetChild(i).position.z);
//			}
//		}
	}

	public Vector3 calculationcForce(Vector3 obj){
		float wellDepth = 128.0f; //constant well depth of argon
		float diameter = 342.0f;  //constant diameter of argon
		float scalePosition = Mathf.Sqrt ( Mathf.Pow((this.transform.position.x - obj.x),2) + 
										   Mathf.Pow((this.transform.position.y - obj.y),2) + 
										   Mathf.Pow((this.transform.position.z - obj.z),2) );
//		Debug.Log ("scalePosition : " + scalePosition);
		float A = 4*wellDepth*Mathf.Pow(diameter,12);
		float B = 4*wellDepth*Mathf.Pow(diameter,6);
		float energy = 12 * A * Mathf.Pow (scalePosition, -14) - 6 * B * Mathf.Pow (scalePosition, -8);
		Debug.Log ("energy "+energy);
		float forceX = energy*(this.transform.position.x - obj.x);
		float forceY = energy*(this.transform.position.y - obj.y);
		float forceZ = energy*(this.transform.position.z - obj.z);
//		Debug.Log ("Force x:"+forceX+" y:"+forceY+" z:"+forceZ);
		return new Vector3(forceX,forceY,forceZ);
	}

	 

//	void OnTriggerEnter(Collider other) {
//		Debug.Log ("xxx");
//		if (other.gameObject.CompareTag ("Argon")) {
//			ArgonScript otherArgon = (ArgonScript)other.gameObject.GetComponent ("ArgonScript");
//		
//			forceVector = getForce (otherArgon);
//			Debug.Log ("x:"+forceVector.x+" y:"+forceVector.y+" z:"+forceVector.x);
//            rb.AddForce(forceVector.x, forceVector.y, forceVector.z);
//		}
//	}

	// Update is called once per frame
	void Update () {
		
		for(int i = 0 ; i < gameController.getNumberArgon() ; i++){
			if(this.transform.position.x != gameController.transform.GetChild (i).position.x){
			listMomentum.Add (momentumVector + 0.5f * Time.deltaTime * forceVector);
//			momentumVector = momentumVector + 0.5f * Time.deltaTime * forceVector;
			positionVector = new Vector3 (gameController.transform.GetChild (i).position.x, gameController.transform.GetChild (i).position.y, gameController.transform.GetChild (i).position.z) + (Time.deltaTime*momentumVector/massArgon);
			Debug.Log ("positionVector x:"+positionVector.x + " y:"+positionVector.y+" z:"+positionVector.z);
			forceVector = calculationcForce (positionVector);
			listMomentum.Add (listMomentum[i] + 0.5f * Time.deltaTime * forceVector);
			listMomentum.RemoveAt (i);
			}
		}
		foreach(Vector3 momentum in listMomentum){
			momentumVector += momentum ;
		}
//		rb.velocity = momentumVector;
//		Debug.Log ("momentum x:" + momentumVector.x +" y:" + momentumVector.y + " z:"+momentumVector.z);
	}

}