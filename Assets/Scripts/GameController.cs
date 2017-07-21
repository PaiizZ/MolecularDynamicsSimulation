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
	private int numberArgon = 10;


	public OxygenScript OxygenPerfab;
	public List<OxygenScript> oxygens = new List<OxygenScript> ();
	private int numberWater = 3;

	//Get GameCotroller for anthor script use
	public static GameController getInstance ()
	{
		return GameObject.Find ("GameController").GetComponent<GameController> ();
	}

	// Use this for initialization
	void Start ()
	{

		for (int i = 0; i < numberWater ; i++) {
		float randomPosX = Random.Range (-4.8f, 4.8f);
		float randomPosY = Random.Range (-4.8f, 4.8f);
		float randomPosZ = Random.Range (-4.8f, 4.8f);
			oxygens.Add (Instantiate (OxygenPerfab, new Vector3 (randomPosX, randomPosY, randomPosZ), Quaternion.identity));
		}

		foreach (OxygenScript oxygen in oxygens) {
			oxygen.transform.SetParent (this.transform);
		}


		Debug.Log ("x : "+ this.gameObject.transform.GetChild (0).position.x + "y : "+ this.gameObject.transform.GetChild (0).position.y + "z : "+ this.gameObject.transform.GetChild (0).position.z);
		Debug.Log ("x : "+ this.gameObject.transform.GetChild (0).GetChild(0).position.x + "y : "+ this.gameObject.transform.GetChild (0).GetChild(0).position.y + "z : "+ this.gameObject.transform.GetChild (0).GetChild(0).position.z);
		Debug.Log ("x : "+ this.gameObject.transform.GetChild (0).position.x + "y : "+ this.gameObject.transform.GetChild (0).position.y + "z : "+ this.gameObject.transform.GetChild (0).position.z);

		//electrostatic ();

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
		if (argonFocus != null) {
			nameText.text = "Name : " + argonFocus.objName;
			moentumText.text = "Momentum : " + argonFocus.momentumVector.magnitude.ToString ("F8") + " Kg·m/mol·s ";
			forceText.text = "Force : " + (argonFocus.objForce.magnitude).ToString ("F15") + " KJ/mol";
			tempText.text = "Temperature " + argonFocus.T + " Kelvins"; 
			positionText.text = "Position  x : " + argonFocus.objPosition.x.ToString ("F3") + " y : " + argonFocus.objPosition.y.ToString ("F3") + " z : " + argonFocus.objPosition.z.ToString ("F3");
		}
	}

	void electrostatic(){
		float Kspring = 9 * Mathf.Pow (10, 15); // (KJnm/c^2)
		float elementaryCharge = 1.602f * Mathf.Pow (10,-19);// c
		float electricChargeOxygen = -0.82f * elementaryCharge; // c
		float electricChargeHydrogen = 0.41f * elementaryCharge; // c
		float numberHydrogeninWater = 2 ;
		List<Vector3> posAtoms = new List<Vector3> ();
		for(int i = 0 ; i < numberWater ; i ++){
			posAtoms.Add (this.gameObject.transform.GetChild (i).position);
			for(int j = 0 ; j < 1 ; j ++){
//				posAtoms.Add (this.gameObject.transform.GetChild(0).GetChild(0).gameObject.transform.position);
			}
		}

		foreach (Vector3 pos in posAtoms) {
			Debug.Log ("x : "+ pos.x + "y : "+ pos.y + "z : "+ pos.z);
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
}
