﻿using System.Collections;
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

    public GameObject partnerOxygen = null;

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

        //rb.velocity = momentumVector;

    }

    // Update is called once per frame
    void Update () {
        //periodicBoundary();
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
