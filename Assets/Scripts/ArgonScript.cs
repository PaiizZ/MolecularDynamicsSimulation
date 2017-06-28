using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgonScript : MonoBehaviour {
	private Rigidbody rb;
	private float R = 8.31447f * Mathf.Pow(10, -3); //molar gas constant ( KJ/mol )
	private float T = 298f ; // temperature in kelvins (25+273)
	private float massArgon = 39.948f/ (6 * Mathf.Pow(10,23)) ; // ( g / molecule )
//	private float massArgon = 39.948f ; // ( g / molecule )
	private float alpha;
	private float beta;
	private float gamma;
	private float calculateValue;
	private float scalar;
    private float velocity;
    private Vector3 unitVector;
	private Vector3 randomVector;
	private Vector3 velocityVector;
	private Vector3 momentumVector;
	private Vector3 forceVector;
	private Vector3 positionVector;
    private Vector3 temp;
    private GameObject go;
	private GameController gameController;
	List<Vector3> listMomentum = new List<Vector3>();
    List<Vector3> listSumMomentum = new List<Vector3>();

    // Use this for initialization
    void Start () {
		GameObject go = GameObject.Find("GameController");
		gameController = (GameController) go.GetComponent(typeof(GameController));

		rb = GetComponent<Rigidbody>();
//		Debug.Log (4*128*Mathf.Pow(342,12)+" eiei " + 4*128*Mathf.Pow(342,6));
		alpha = Random.Range (-5.0f, 5.0f);
		beta = Random.Range (-5.0f, 5.0f);
		gamma = Random.Range (-5.0f, 5.0f);

		randomVector = new Vector3 (alpha,beta,gamma);

		calculateValue = Mathf.Pow (alpha, 2) + Mathf.Pow (beta, 2) + Mathf.Pow (gamma, 2);
	//	Debug.Log ("calculateValue: " + calculateValue);

		scalar = Mathf.Sqrt (calculateValue);
	//	Debug.Log ("scaleVector: " + scalar);

		unitVector = ( 1 / scalar) * randomVector;
        //		Debug.Log ("unitVector: " + unitVector);
        velocity = Mathf.Sqrt((3 * R * T) / (massArgon * calculateValue));
    //    Debug.Log("velocity :" + velocity);
        velocityVector = velocity * randomVector;
	//	Debug.Log ("velocity x:" + velocityVector.x +" y:" + velocityVector.y + " z:"+velocityVector.z);

		momentumVector = massArgon * velocityVector;
   	//	Debug.Log ("momentum x:" + momentumVector.x +" y:" + momentumVector.y + " z:"+momentumVector.z);

        //	rb.velocity = momentumVector;
        	//for(int i = 0 ; i < gameController.getNumberArgon() ; i++){
        //			if(this.transform.position.x != gameController.gameObject.transform.GetChild(i).position.x){
        	//	Debug.Log ("x:" + gameController.gameObject.transform.GetChild(i).position.x+" y:" +  gameController.gameObject.transform.GetChild(i).position.y+" z:" +  gameController.gameObject.transform.GetChild(i).position.z);
        	//		}
        	//	}
    }

    public Vector3 calculationcForce(Vector3 obj){
		float wellDepth = 128f * Mathf.Pow(10,-3); //constant well depth of argon (KJ/mol)
		float diameter = 3.42f ;  //constant diameter of argon (Angstrom)  
    //    Debug.Log("wellDepth " + wellDepth);
    //    Debug.Log("diameter " + diameter);
    //    Debug.Log("scalePosition x :" + this.transform.position.x +", other :" +obj.x + " y :" + this.transform.position.y + ", other :" + obj.y + " z : other :" + this.transform.position.z + "," + obj.z);
        float scalePosition = (Mathf.Sqrt ( Mathf.Pow((this.transform.position.x - obj.x),2) + 
										   Mathf.Pow((this.transform.position.y - obj.y),2) + 
										   Mathf.Pow((this.transform.position.z - obj.z),2) )) ;
	//	Debug.Log ("scalePosition : " + scalePosition);
	//	float A = 4 * wellDepth * Mathf.Pow(diameter, 12);
    //    Debug.Log("A " + A);
	//	float B = 4 * wellDepth * Mathf.Pow(diameter, 6);
    //    Debug.Log("B " + B);
		float energy = 12 * 4 * wellDepth * Mathf.Pow(diameter, 12) * Mathf.Pow (scalePosition, -14) - 6 * 4 * wellDepth * Mathf.Pow(diameter, 6) * Mathf.Pow (scalePosition, -8);
		Debug.Log ("energy "+energy);
		float forceX = energy*(this.transform.position.x - obj.x);
		float forceY = energy*(this.transform.position.y - obj.y);
		float forceZ = energy*(this.transform.position.z - obj.z);
		Debug.Log ("Force x:"+forceX+" y:"+forceY+" z:"+forceZ);
		return new Vector3(forceX,forceY,forceZ);
	}

	// Update is called once per frame
	void Update () {
		for(int i = 0 ; i < gameController.getNumberArgon() ; i++){
			if(this.transform.position.x != gameController.transform.GetChild (i).position.x &&
                this.transform.position.y != gameController.transform.GetChild(i).position.y &&
                this.transform.position.z != gameController.transform.GetChild(i).position.z){
   
                positionVector = new Vector3 (gameController.transform.GetChild (i).position.x, gameController.transform.GetChild (i).position.y, gameController.transform.GetChild (i).position.z) ;
                Debug.Log("x:" + positionVector.x + " y:" + positionVector.y + " z:" + positionVector.z);
          
                forceVector += calculationcForce (positionVector);
            }
		}
        Debug.Log("forceVector x:" + forceVector.x + " y:" + forceVector.y + " z:" + forceVector.z);
        rb.AddForce(forceVector);
        forceVector.Set(0f, 0f, 0f);


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
}