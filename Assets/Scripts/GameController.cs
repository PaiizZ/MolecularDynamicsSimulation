using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

	//model of molecule that use to cloning
	public ArgonScript argonPerfab;
	//List of atoms that created
	public List<ArgonScript> argons = new List<ArgonScript> ();
	//another Text that show on Sence
	public Text nameText, moentumText, forceText, tempText, positionText;
	// private number of argon atoms
	private int numberArgon = 100;

	//model of molecule that use to cloning
	public OxygenScript OxygenPerfab;
	//List of molecules that created
	public List<OxygenScript> oxygens = new List<OxygenScript> ();
	// private number of water molecules
	private int numberWater = 5;

	//focus molecule about information
	private ArgonScript argonFocus;
	
	private OxygenScript waterFocus;

	//Get GameCotroller for anthor script use
	public static GameController getInstance ()
	{
		return GameObject.Find ("GameController").GetComponent<GameController> ();
	}

	// Use this for initialization
	void Start ()
	{

		for (int i = 0; i < numberWater; i++) {
			float randomPosX = Random.Range (-1.44f, 1.44f);
			float randomPosY = Random.Range (-1.44f, 1.44f);
			float randomPosZ = Random.Range (-1.44f, 1.44f);
			oxygens.Add (Instantiate (OxygenPerfab, new Vector3 (randomPosX, randomPosY, randomPosZ), Quaternion.identity));
		}

		foreach (OxygenScript oxygen in oxygens) {
			oxygen.transform.SetParent (this.transform);
		}
			
//		for (int i = 0; i < numberArgon; i++) {
//			float randomPosX = Random.Range (-4.8f, 4.8f);
//			float randomPosY = Random.Range (-4.8f, 4.8f);
//			float randomPosZ = Random.Range (-4.8f, 4.8f);
//			argons.Add (Instantiate (argonPerfab, new Vector3 (randomPosX, randomPosY, randomPosZ), Quaternion.identity));
//		}
//		
//		foreach (ArgonScript argon in argons) {
//			argon.transform.SetParent (this.transform);
//		}

	}

	// Update is called once per frame
	void Update ()
	{
		if (waterFocus != null) {
			nameText.text = "Name : " + waterFocus.objName;
			moentumText.text = "Momentum : " + waterFocus.momentumVector.magnitude.ToString ("F8") + " Kg·m/mol·s ";
			forceText.text = "Force : " + (waterFocus.objForce.magnitude).ToString ("F15") + " KJ/mol";
			tempText.text = "Temperature " + waterFocus.T + " Kelvins"; 
			positionText.text = "Position  x : " + waterFocus.objPosition.x.ToString ("F3") + " y : " + waterFocus.objPosition.y.ToString ("F3") + " z : " + waterFocus.objPosition.z.ToString ("F3");
		}
		
		if (argonFocus != null) {
			nameText.text = "Name : " + argonFocus.objName;
			moentumText.text = "Momentum : " + argonFocus.momentumVector.magnitude.ToString ("F8") + " Kg·m/mol·s ";
			forceText.text = "Force : " + (argonFocus.objForce.magnitude).ToString ("F15") + " KJ/mol";
			tempText.text = "Temperature " + argonFocus.T + " Kelvins"; 
			positionText.text = "Position  x : " + argonFocus.objPosition.x.ToString ("F3") + " y : " + argonFocus.objPosition.y.ToString ("F3") + " z : " + argonFocus.objPosition.z.ToString ("F3");
		}

	}

	//Get number of Argon molecule
	public int getNumberArgon ()
	{
		return this.numberArgon;
	}

	//Get number of Oxygen molecule
	public int getNumberOxygen ()
	{
		return this.numberWater;
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
	
	//Change focus of molecule to anothor molecule
	public void changeWaterFocus (OxygenScript molecule)
	{
		if (this.waterFocus == null) {
			this.waterFocus = molecule;
		} else {
			this.waterFocus.changeOnClick ();
			this.waterFocus = molecule;
		}
	}
}
