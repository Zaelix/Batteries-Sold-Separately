using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour{
	GameObject cam;
	GameObject selectionBox;
	GameObject selection;
	GameObject[,] floorMap = new GameObject[50,50];
	GameObject[,] factoryMap = new GameObject[50,50];
	Dictionary<string, double> costs = new Dictionary<string,double> ();
	bool isBuilding = false;
	string buildingType = "";

	double pricePerKW = 0.02;
	double powerTotal = 0;
	double moneyTotal = 1000;

	// Use this for initialization
	void Start () {
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		InitializeFloor ();
		InitializeCosts ();
		selectionBox = GameObject.FindGameObjectWithTag ("Player");
	}

	private void InitializeFloor(){
		for (int x = 0; x < 50; x++) {
			for (int y = 0; y < 50; y++) {
				floorMap [x, y] = CreateFloorObject (x, y);
			}
		}
	}

	private void InitializeCosts(){
		costs.Add ("", 9999999);
		costs.Add ("Boiler", 300);
		costs.Add ("Turbine", 500);
	}

	// Update is called once per frame
	void Update () {
		GetInput ();
		CountPowerProduced ();
	}

	void FixedUpdate(){
		moneyTotal += (powerTotal * pricePerKW)/10.0;
	}

	private void CountPowerProduced(){
		double power = 0;
		for (int x = 0; x < 50; x++) {
			for (int y = 0; y < 50; y++) {
				if (factoryMap [x, y] != null) {
					Tile tile = factoryMap [x, y].GetComponent<Tile> ();
					power += tile.KwProduced;
				}
			}
		}
		powerTotal = power;
	}

	private void GetInput(){
		// Movement
		if (Input.GetKey (KeyCode.W)) {
			cam.transform.position += new Vector3(0,0.2f,0); 
		}

		if (Input.GetKey (KeyCode.S)) {
			cam.transform.position -= new Vector3(0,0.2f,0); 
		}

		if (Input.GetKey (KeyCode.A)) {
			cam.transform.position -= new Vector3(0.2f,0,0); 
		}

		if (Input.GetKey (KeyCode.D)) {
			cam.transform.position += new Vector3(0.2f,0,0); 
		}

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			isBuilding = true;
			buildingType = "Boiler";
		}
		else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			isBuilding = true;
			buildingType = "Turbine";
		}

		// Mouse Input
		if (Input.GetMouseButtonDown (0) && isBuilding == true && selection.GetComponent<Tile>().IsBuildable() && moneyTotal >= costs[buildingType]) {
			int x = (int)selection.transform.position.x;
			int y = (int)selection.transform.position.y;
			factoryMap [x, y] = CreateMachineObject (buildingType, x, y);
			moneyTotal -= costs[buildingType];
		}

		if (Input.GetMouseButtonUp (1) || Input.GetKeyDown(KeyCode.Escape)) {
			isBuilding = false;
		}
	}

	private GameObject CreateMachineObject(string bName, int x, int y){
		GameObject tile = (GameObject)Instantiate(Resources.Load(bName), new Vector2(x,y), Quaternion.identity);
		tile.transform.position -= new Vector3(0,0,0.1f);
		Tile t = tile.GetComponent<Tile> ();
		t.SetTileName (bName);
		t.SetCost (costs[bName]);
		t.SetBuildable (false);
		t.SetManager (this.gameObject);
		return tile;
	}

	private GameObject CreateFloorObject(int x, int y){
		GameObject tile = (GameObject)Instantiate(Resources.Load("Metal"), new Vector2(x,y), Quaternion.identity);
		Tile t = tile.GetComponent<Tile> ();
		t.SetTileName ("Metal");
		t.SetCost (10);
		t.SetBuildable (true);
		t.SetManager (this.gameObject);
		return tile;
	}

	public void SetSelection(GameObject s){
		this.selection = s;
		selectionBox.transform.position = s.transform.position - new Vector3 (0,0,0.1f);
	}

	public GameObject GetSelection(){
		return this.selection;
	}

	public void SetSelectionColor(Color color){
		selectionBox.GetComponent<Renderer> ().material.color = color;
	}

	public GameObject GetSelectionBox(){
		return selectionBox;
	}

	public bool GetBuildStatus(){
		return isBuilding;
	}

	public double PowerTotal {
		get {
			return powerTotal;
		}
	}

	public double MoneyTotal {
		get {
			return moneyTotal;
		}
	}
}
