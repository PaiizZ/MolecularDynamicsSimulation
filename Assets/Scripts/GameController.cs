using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public ArgonScript argonPerfab;
	public List<ArgonScript> argons = new List<ArgonScript> ();
	// private number of argon molecular
	private int numberArgon = 20 ;
	// Use this for initialization
	void Start () {
		for(int i = 0 ; i < numberArgon ; i++){
			argons.Add (Instantiate(argonPerfab,new Vector3(Random.Range(-14.0f, 14.0f),Random.Range(-14.0f, 14.0f),Random.Range(-14.0f, 14.0f)),Quaternion.identity));
		}
		//foreach (ArgonScript argon in argons) {
		//	argon.GetComponent<Rigidbody> ().velocity = new Vector3 (Random.Range (-20.0f, 20.0f),
		//		Random.Range (-20.0f, 20.0f),
		//		Random.Range (-20.0f, 20.0f));
		//}
	}

	// Update is called once per frame
	void Update () {

	}
}
