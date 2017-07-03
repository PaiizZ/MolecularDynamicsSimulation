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
			argons.Add (Instantiate(argonPerfab,new Vector3(Random.Range(-4.8f, 4.8f),Random.Range(-4.8f, 4.8f),Random.Range(-4.8f, 4.8f)),Quaternion.identity));
		}

		foreach (ArgonScript argon in argons) {
			argon.transform.SetParent (this.transform);
		}

//		for(int i = 0 ; i < numberArgon ; i++){
//			Debug.Log ("x:" + this.transform.GetChild(i).position.x+" y:" +  this.transform.GetChild(i).position.y+" z:" +  this.transform.GetChild(i).position.z);
//		}
	}

	// Update is called once per frame
	void Update () {

	}

	public int getNumberArgon(){
		return this.numberArgon;
	}
}
