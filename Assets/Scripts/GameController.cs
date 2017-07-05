using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public ArgonScript argonPerfab;
    private ArgonScript argonFocus;
	public List<ArgonScript> argons = new List<ArgonScript> ();

    public Text nameText, moentumText, forceText, tempText, positionText;

	// private number of argon molecular

	private int numberArgon = 80 ;

  

	// Use this for initialization
	void Start () {
		for(int i = 0 ; i < numberArgon ; i++){
			argons.Add (Instantiate(argonPerfab,new Vector3(Random.Range(-4.8f, 4.8f),Random.Range(-4.8f, 4.8f),Random.Range(-4.8f, 4.8f)),Quaternion.identity));
		}

		foreach (ArgonScript argon in argons) {
			argon.transform.SetParent (this.transform);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public int getNumberArgon(){
		return this.numberArgon;
	}

  
}
