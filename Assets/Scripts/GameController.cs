using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public ArgonScripts argonPerfab;
	public List<ArgonScripts> argons = new List<ArgonScripts> ();
	// private number of argon molecular
	private int numberArgon = 10 ;
	// Use this for initialization
	void Start () {
		for(int i = 0 ; i < numberArgon ; i++){
			argons.Add (Instantiate(argonPerfab,new Vector3(Random.Range(-9.0f, 9.0f),Random.Range(-9.0f, 9.0f),Random.Range(-9.0f, 9.0f)),Quaternion.identity));
		}
//		foreach (ArgonScripts argon in argons)
//		{
//			argon.GetComponent<Rigidbody>().velocity = new Vector3( Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f) );
//		}
	}

	// Update is called once per frame
	void Update () {

	}
}
