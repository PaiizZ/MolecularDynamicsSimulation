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
    private float maxDistance = 2.5f;
    private Vector3 unitVector;
    private Vector3 randomVector;
    private Vector3 velocityVector;
    private Vector3 momentumVector;
    private Vector3 forceTotal;
    private Vector3 forceVector;
    private Vector3 positionVector;
    private GameObject go;
    private GameController gameController;
    private Vector3 tempObjectPosition;
    private Vector3 objForce;
    private Vector3[] forceFromObj;
    private Transform otherTransformObj;
    private int numberOfMolecule;
    private float time;

    // Use this for initialization
    void Start() {
        GameObject go = GameObject.Find("GameController");
        gameController = (GameController)go.GetComponent(typeof(GameController));

        rb = GetComponent<Rigidbody>();

        this.numberOfMolecule = gameController.getNumberArgon();

        this.objForce = new Vector3();

        this.forceFromObj = new Vector3[this.numberOfMolecule+ 1];

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

        rb.velocity = momentumVector;
        //for(int i = 0 ; i < gameController.getNumberArgon() ; i++){
        //			if(this.transform.position.x != gameController.gameObject.transform.GetChild(i).position.x){
        //	        Debug.Log ("x:" + gameController.gameObject.transform.GetChild(i).position.x+" y:" +  gameController.gameObject.transform.GetChild(i).position.y+" z:" +  gameController.gameObject.transform.GetChild(i).position.z);
        //		}
        //	}
    }



    // Update is called once per frame
    void Update() {
       setTempPosition();
       this.time = Time.deltaTime * Mathf.Pow(10, -12);
        momentumVector = momentumVector + (0.5f * time * forceVector);

        for (int i = 0; i < this.numberOfMolecule ; i++) {
            this.otherTransformObj = gameController.transform.GetChild(i);
            if (this.transform.position.x != otherTransformObj.position.x &&
                this.transform.position.y != otherTransformObj.position.y &&
                this.transform.position.z != otherTransformObj.position.z) {

                //positionVector = new Vector3(gameController.transform.GetChild(i).position.x, gameController.transform.GetChild(i).position.y, gameController.transform.GetChild(i).position.z);
                //positionVector = positionVector +( (1f *Time.deltaTime * Mathf.Pow(10, -12)) / massArgon);
                //Debug.Log("x:" + positionVector.x + " y:" + positionVector.y + " z:" + positionVector.z);

                calculationcForce(otherTransformObj, time);
            }
        }


        periodicBoundary();
        //forceVector.Set(0f, 0f, 0f);
    


    }


    public void calculationcForce(Transform obj, float time)
    {
        float wellDepth = 0.128f; //constant well depth of argon (KJ/mol)
        float diameter = 3.42f;  //constant diameter of argon (Angstrom)  
        Vector3 position = transform.position;
        Vector3 tempPosition = obj.position;
        periodicBoundary();
        //    Debug.Log("wellDepth " + wellDepth);
        //    Debug.Log("diameter " + diameter);
        //    Debug.Log("scalePosition x :" + this.transform.position.x +", other :" +obj.x + " y :" + this.transform.position.y + ", other :" + obj.y + " z : other :" + this.transform.position.z + "," + obj.z);
        float distance = (Mathf.Sqrt(Mathf.Pow((tempPosition.x - position.x), 2) +
                                           Mathf.Pow((tempPosition.y - position.y), 2) +
                                           Mathf.Pow((tempPosition.z - position.z), 2)));
        // Debug.Log("distance " + distance);
        Vector3 temp = new Vector3(distance, distance, distance) + (time * this.momentumVector / massArgon);
        float scalarDistance = (Mathf.Sqrt(Mathf.Pow((temp.x), 2) + Mathf.Pow((temp.y), 2) + Mathf.Pow((temp.z), 2)));


        float distance2 = (Mathf.Sqrt(Mathf.Pow((tempPosition.x - tempObjectPosition.x), 2) +
                                          Mathf.Pow((tempPosition.y - tempObjectPosition.y), 2) +
                                          Mathf.Pow((tempPosition.z - tempObjectPosition.z), 2)));
        Vector3 temp2 = new Vector3(distance2, distance2, distance2) + (time * this.momentumVector / massArgon);
        float scalarDistance2 = (Mathf.Sqrt(Mathf.Pow((temp2.x), 2) + Mathf.Pow((temp2.y), 2) + Mathf.Pow((temp2.z), 2)));
        // Debug.Log ("scalarDistance : " + scalarDistance);
        // if (distance > maxDistance){
        //    float energy = 12 * 4 * wellDepth * Mathf.Pow(diameter, 12) * Mathf.Pow(distance, -14) - 6 * 4 * wellDepth * Mathf.Pow(diameter, 6) * Mathf.Pow(distance, -8);
        //    rb.AddForce( energy * (tempPosition - position));
        //}
        if (scalarDistance <= maxDistance)
        {
            // Debug.Log("scalarDistance " + scalarDistance);
            float energy = 12 * 4 * wellDepth * Mathf.Pow(diameter, 12) * Mathf.Pow(scalarDistance, -14) - 6 * 4 * wellDepth * Mathf.Pow(diameter, 6) * Mathf.Pow(scalarDistance, -8);
            // float energy = (4 * 3) * (Mathf.Pow((4 / scalarDistance), 12) - ( Mathf.Pow((4 / scalarDistance), 6)));
            // Debug.Log("energy " + energy);
            // Debug.Log ("equation " + equation);
            rb.AddForce(0.5f * (-energy * (tempPosition - position) * time));
        }
        else if (scalarDistance2 <= maxDistance)
        {
            float energy = 12 * 4 * wellDepth * Mathf.Pow(diameter, 12) * Mathf.Pow(scalarDistance2, -14) - 6 * 4 * wellDepth * Mathf.Pow(diameter, 6) * Mathf.Pow(scalarDistance2, -8);
            rb.AddForce(0.5f * (-energy * (tempPosition - tempObjectPosition) * time));
        }
        else
        {
            float energy = 12 * 4 * wellDepth * Mathf.Pow(diameter, 12) * Mathf.Pow(scalarDistance, -14) - 6 * 4 * wellDepth * Mathf.Pow(diameter, 6) * Mathf.Pow(scalarDistance, -8);
            rb.AddForce(0.5f * (energy * (tempPosition - position) * time));
        }
       
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