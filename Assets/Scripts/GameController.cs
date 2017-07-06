using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

	//model of molecule that use to cloning
	public ArgonScript argonPerfab;
	//focus molecule about information
	private ArgonScript argonFocus;
	//List of molecules that created
	public List<ArgonScript> argons = new List<ArgonScript> ();
	//another Text that show on Sence
	public Text nameText, moentumText, forceText, tempText, positionText;
	// private number of argon molecules
	private int numberArgon = 80;

	//Get GameCotroller for anthor script use
	public static GameController getInstance ()
	{
		return GameObject.Find ("GameController").GetComponent<GameController> ();
	}

	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < numberArgon; i++) {
			argons.Add (Instantiate (argonPerfab, new Vector3 (Random.Range (-4.8f, 4.8f), Random.Range (-4.8f, 4.8f), Random.Range (-4.8f, 4.8f)), Quaternion.identity));
		}

		foreach (ArgonScript argon in argons) {
			argon.transform.SetParent (this.transform);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (argonFocus != null) {
			nameText.text = "Name : " + argonFocus.objName;
			moentumText.text = "Momentum : " + argonFocus.momentumVector.magnitude.ToString ("F8") + " Kg·m/mol·s ";
			forceText.text = "Force : " + (argonFocus.objForce.magnitude).ToString ("F15") + " KJ/mol";
			tempText.text = "Temperature " + argonFocus.T + " Kelvins"; 
			positionText.text = "Position  x : " + argonFocus.objPosition.x.ToString ("F3") + " y : " + argonFocus.objPosition.y.ToString ("F3") + " z : " + argonFocus.objPosition.z.ToString ("F3");
		}
	}

	//Get number of molecule
	public int getNumberArgon ()
	{
		return this.numberArgon;
	}

	//Change focus of molecule to anothor molecule
	public void changeArgonFocus (ArgonScript molecule)
	{
		if (this.argonFocus == null) {
			this.argonFocus = molecule;
		} else {
			this.argonFocus.changeOnClick ();
			this.argonFocus = molecule;
		}
	}
}
