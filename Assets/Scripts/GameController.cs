using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public ArgonScript argonPerfab;
    private ArgonScript argonFocus;
	public List<ArgonScript> argons = new List<ArgonScript> ();

    //another Text that show on Sence
    public Text nameText, moentumText, forceText, tempText, positionText;

    // private number of argon molecular
    private int numberArgon = 80;

    public static GameController getInstance()
    {
        return GameObject.Find("GameController").GetComponent<GameController>();
    }

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
        if (argonFocus != null)
        {
            nameText.text = "Name : " + argonFocus.objName;
            forceText.text = "Force : " + (argonFocus.objForce.magnitude * 5f).ToString("F10") + " N";
            //Vector3 argonPosition = argonFocus.transform.position;
            positionText.text = "Position  x : " + argonFocus.objPosition.x.ToString("F3") + " y : " + argonFocus.objPosition.y + " z : " + argonFocus.objPosition.z;
        }
    }

	public int getNumberArgon(){
		return this.numberArgon;
	}

    public void changeArgonFocus(ArgonScript molecule)
    {
        if (this.argonFocus == null)
        {
            this.argonFocus = molecule;
        }
        else
        {
            this.argonFocus.changeOnClick();
            this.argonFocus = molecule;
        }
    }
}
