using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject gameObject;
	public ArgonScript argonPerfab;
	public List<ArgonScript> argons = new List<ArgonScript> ();
	// private number of argon molecular
	private int numberArgon = 1 ;
	private static GameController instance; 

	public static GameController getInstance(){
		if (instance == null) instance = new GameController ();
		return instance;
	}
	// Use this for initialization
	void Start () {
		for(int i = 0 ; i < numberArgon ; i++){
			argons.Add (Instantiate(argonPerfab,new Vector3(Random.Range(-14.0f, 14.0f),Random.Range(-14.0f, 14.0f),Random.Range(-14.0f, 14.0f)),Quaternion.identity));
		}

		foreach (ArgonScript argon in argons) {
			argon.transform.SetParent (gameObject.transform);
		}

//		for(int i = 0 ; i < numberArgon ; i++){
//			Debug.Log ("x:" + gameObject.transform.GetChild(i).position.x+" y:" +  gameObject.transform.GetChild(i).position.y+" z:" +  gameObject.transform.GetChild(i).position.z);
//		}
	}

	// Update is called once per frame
	void Update () {

	}

	public int getNumberArgon(){
		return this.numberArgon;
	}
}
