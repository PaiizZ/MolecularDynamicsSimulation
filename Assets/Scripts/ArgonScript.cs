using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgonScript : MonoBehaviour {
    private Rigidbody rb;
    private float R = 8.31447f * Mathf.Pow(10, -3); //molar gas constant ( KJ/mol )
    private float T = 298f; // temperature in kelvins (25+273)
                            //private float massArgon = 39.948f/ (6 * Mathf.Pow(10,23)) ; // ( g / molecule )
    private float massArgon = 39.948f * Mathf.Pow(10, -3); // ( Kg / molecule )
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
    private Vector3 forceTotal;
    private Vector3 forceVector;
    private Vector3 positionVector;
    private Vector3 temp;
    private GameObject go;
    private GameController gameController;
    private Vector3 tempObjectPosition;
   
    private float maxDistance = 2f;
    private bool start = true;

    // Use this for initialization
    void Start() {
        GameObject go = GameObject.Find("GameController");
        gameController = (GameController)go.GetComponent(typeof(GameController));

        rb = GetComponent<Rigidbody>();
        //		Debug.Log (4*128*Mathf.Pow(342,12)+" eiei " + 4*128*Mathf.Pow(342,6));
        alpha = Random.Range(-3.0f, 3.0f);
        beta = Random.Range(-3.0f, 3.0f);
        gamma = Random.Range(-3.0f, 3.0f);

        randomVector = new Vector3(alpha, beta, gamma);

        calculateValue = Mathf.Pow(alpha, 2) + Mathf.Pow(beta, 2) + Mathf.Pow(gamma, 2);
        //	Debug.Log ("calculateValue: " + calculateValue);

        scalar = Mathf.Sqrt(calculateValue);
        //	Debug.Log ("scaleVector: " + scalar);

        unitVector = (1 / scalar) * randomVector;
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



    // Update is called once per frame
    void Update() {
        float time = Time.deltaTime * Mathf.Pow(10, -12);
        momentumVector = momentumVector + (0.5f * time * forceVector);

        for (int i = 0; i < gameController.getNumberArgon(); i++) {
            periodicBoundary();
            if (this.transform.position.x != gameController.transform.GetChild(i).position.x &&
                this.transform.position.y != gameController.transform.GetChild(i).position.y &&
                this.transform.position.z != gameController.transform.GetChild(i).position.z) {

                positionVector = new Vector3(gameController.transform.GetChild(i).position.x, gameController.transform.GetChild(i).position.y, gameController.transform.GetChild(i).position.z);
                //positionVector = positionVector +( (1f *Time.deltaTime * Mathf.Pow(10, -12)) / massArgon);
                //Debug.Log("x:" + positionVector.x + " y:" + positionVector.y + " z:" + positionVector.z);

                forceTotal += calculationcForce(gameController.transform.GetChild(i), time);
            }
        }
        //Debug.Log("forceTotal x:" + forceTotal.x + " y:" + forceTotal.y + " z:" + forceTotal.z);
        forceVector += forceTotal;
        //Debug.Log("forceVector x:" + forceVector.x + " y:" + forceVector.y + " z:" + forceVector.z);
        forceTotal.Set(0f, 0f, 0f);
        momentumVector = momentumVector + (0.5f * time * forceVector);
        //Debug.Log("momentumVector x:" + momentumVector.x + " y:" + momentumVector.y + " z:" + momentumVector.z);
        forceTotal.Set(0f, 0f, 0f);
        forceVector.Set(0f, 0f, 0f);
        rb.velocity = momentumVector;
        periodicBoundary();
        //forceVector.Set(0f, 0f, 0f);


    }

    public Vector3 calculationcForce(Transform obj, float time)
    {
        setTempPosition();
        float energy = 0f;
        Vector3 force;
        float wellDepth = 128f * Mathf.Pow(10, -3); //constant well depth of argon (KJ/mol)
        float diameter = 3.42f;  //constant diameter of argon (Angstrom)  
                                 //    Debug.Log("wellDepth " + wellDepth);
                                 //    Debug.Log("diameter " + diameter);
                                 //Debug.Log("scalePosition x :" + this.transform.position.x +", other :" +obj.x + " y :" + this.transform.position.y + ", other :" + obj.y + " z : other :" + this.transform.position.z + "," + obj.z);
        Vector3 distanceVector = obj.position - this.transform.position;
        float distance = (Mathf.Sqrt(Mathf.Pow((this.transform.position.x - obj.position.x), 2) +
                                           Mathf.Pow((this.transform.position.y - obj.position.y), 2) +
                                           Mathf.Pow((this.transform.position.z - obj.position.z), 2)));

        //temp = new Vector3(distance, distance, distance) + (time * momentumVector / massArgon);
        //float scalar = (Mathf.Sqrt(Mathf.Pow((temp.x), 2) + Mathf.Pow((temp.y), 2) + Mathf.Pow((temp.z), 2)));

        //Debug.Log ("scalePosition : " + distance);
        //Debug.Log("scalar : " + scalar);
        //	  float A = 4 * wellDepth * Mathf.Pow(diameter, 12);
        //    Debug.Log("A " + A);
        //	  float B = 4 * wellDepth * Mathf.Pow(diameter, 6);
        //    Debug.Log("B " + B);
       
        if (distance <= 10f)
        {
            // Debug.Log("energy " + energy);
            energy = 12 * 4 * wellDepth * Mathf.Pow(diameter, 12) * Mathf.Pow(scalar, -14) - 6 * 4 * wellDepth * Mathf.Pow(diameter, 6) * Mathf.Pow(scalar, -8);
            force = -energy * distanceVector;
      
        }
        else
        {
            energy = 12 * 4 * wellDepth * Mathf.Pow(diameter, 12) * Mathf.Pow(scalar, -14) - 6 * 4 * wellDepth * Mathf.Pow(diameter, 6) * Mathf.Pow(scalar, -8);
            force = energy * distanceVector;
           
        }

        //Debug.Log ("Force x:"+forceX+" y:"+forceY+" z:"+forceZ);
        return force;
    }

    void periodicBoundary()
    {
        Vector3 position = this.transform.position;
        if (position.x >= 5f)
        {
            position.x = -5f;
        }
        else if (position.x <= -5f)
        {
            position.x = 5f;
        }
        else if (position.y >= 5f)
        {
            position.y = -5f;
        }
        else if (position.y <= -5f)
        {
            position.y = 5f;
        }
        else if (position.z >= 5f)
        {
            position.z = -5f;
        }
        else if (position.z <= -5f)
        {
            position.z = 5f;
        }
        rb.MovePosition(position);
    }

    void setTempPosition()
    {
        this.tempObjectPosition = this.transform.position;
        if (this.tempObjectPosition.x == Mathf.Abs(this.tempObjectPosition.x)){
            this.tempObjectPosition.x -= 10 ;
        }
        else{
            this.tempObjectPosition.x += 10;
        }
        if (this.tempObjectPosition.y == Mathf.Abs(this.tempObjectPosition.y))
        {
            this.tempObjectPosition.y -= 10;
        }
        else
        {
            this.tempObjectPosition.y += 10;
        }
        if (this.tempObjectPosition.z == Mathf.Abs(this.tempObjectPosition.z))
        {
            this.tempObjectPosition.z -= 10;
        }
        else
        {
            this.tempObjectPosition.z += 10;
        }
    }
}