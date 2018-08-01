using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour{
	GameObject cam;
	GameObject selectionBox;
	GameObject selection;
    GameObject buildGhost;
	GameObject[,] floorMap = new GameObject[50,50];
	GameObject[,] factoryMap = new GameObject[50,50];
    Dictionary<string, double> costs = new Dictionary<string, double>();
    Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    bool isBuilding = false;
	string buildingType = "";

    double kwhPerKGCoal = 8.142;
    double priceOfCoal = 0.06;
	double pricePerKW = 0.02;
	double powerTotal = 0;
	double moneyTotal = 10000;

    // Efficiencies
    double coalBurningEfficiency = 0.70;

    double[] marketDemandCurve = new double[] { 82, 77, 74, 71, 72, 73, 75, 77, 80, 82, 85, 87, 89, 92, 94, 96, 98, 99, 100, 100, 95, 92, 88, 83, 82 };
    double currentDemand = 0;
    double marketPowerDemand = 10000;
    double marketPowerSupply = 0;
    double competitorPowerGenerated = 5000;
    double totalMaintenanceCost = 0;

    // Reference vars
    private int time;
    private double kwProducedToday = 0;

	// Use this for initialization
	void Start () {
        Tile.SetManager(this.gameObject);
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		InitializeFloor ();
		InitializeDictionaries ();
		selectionBox = GameObject.FindGameObjectWithTag ("Player");
        foreach (Transform child in selectionBox.transform)
        {
            if (child.tag != "Player")
            {
                buildGhost = child.gameObject;
            }
        }
    }

	private void InitializeFloor(){
		for (int x = 0; x < 50; x++) {
			for (int y = 0; y < 50; y++) {
				floorMap [x, y] = CreateFloorObject (x, y);
			}
		}
	}

	private void InitializeDictionaries(){
		costs.Add ("", 9999999);
        sprites.Add("", Resources.Load<Sprite>("images/select"));
		costs.Add ("Boiler", 300);
        sprites.Add("Boiler", Resources.Load<Sprite>("images/Boiler"));
        costs.Add ("Turbine", 500);
        sprites.Add("Turbine", Resources.Load<Sprite>("images/Turbine"));
    }

	// Update is called once per frame
	void Update () {
		GetInput ();
		//CountPowerProduced ();
        CalculateMarketDemand();
    }

	void FixedUpdate(){
		//moneyTotal += (powerTotal * pricePerKW)/10.0;
	}

    public void PerformMaintenance()
    {
        moneyTotal -= CalculateDailyCosts();
        Debug.Log("Daily Maintenance performed! Cost: " + totalMaintenanceCost);
    }

    public void CalculateDailyIncome()
    {
        Debug.Log("Produced " + kwProducedToday + " kw/h today.");
        moneyTotal += kwProducedToday * pricePerKW;
        Debug.Log("$" + kwProducedToday * pricePerKW + " earned.");
        kwProducedToday = 0;
    }

    private double CalculateDailyCosts()
    {
        double totalCosts = 0;
        for (int x = 0; x < 50; x++)
        {
            for (int y = 0; y < 50; y++)
            {
                if (factoryMap[x, y] != null)
                {
                    Tile tile = factoryMap[x, y].GetComponent<Tile>();
                    totalCosts += tile.PerformDailyMaintenance();
                }
            }
        }
        totalMaintenanceCost = totalCosts;
        return totalCosts;
    }

    private void CalculateMarketDemand()
    {
        time = (int)this.gameObject.GetComponent<GUIHandler>().GetTime();
        currentDemand = marketPowerDemand * (marketDemandCurve[time] / 100);
        marketPowerSupply = competitorPowerGenerated + powerTotal;
        pricePerKW = (currentDemand / marketPowerSupply)*0.02;
    }

	public double CountPowerProduced(){
		float power = 0;
		for (int x = 0; x < 50; x++) {
			for (int y = 0; y < 50; y++) {
				if (factoryMap [x, y] != null) {
					Tile tile = factoryMap [x, y].GetComponent<Tile> ();
					power += (float)tile.KwProduced;
				}
			}
		}
        kwProducedToday += power;
        powerTotal = power;
        return power;
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
            SetBuilding("Boiler");
		}
		else if (Input.GetKeyDown (KeyCode.Alpha2))
        {
            SetBuilding("Turbine");
        }

		// Mouse Input
		if (Input.GetMouseButtonDown (0) && isBuilding == true && selection.GetComponent<Tile>().IsBuildable() && moneyTotal >= costs[buildingType]) {
			int x = (int)selection.transform.position.x;
			int y = (int)selection.transform.position.y;
			factoryMap [x, y] = CreateMachineObject (buildingType, x, y);
			moneyTotal -= costs[buildingType];
		}

		if (Input.GetMouseButtonUp (1) || Input.GetKeyDown(KeyCode.Escape)) {
            SetBuilding("");
			isBuilding = false;
		}
	}

    private void SetBuilding(string type)
    {
        isBuilding = true;
        buildingType = type;
        buildGhost.GetComponent<SpriteRenderer>().sprite = sprites[type];
        //Debug.Log(sprites[type]);
    }

	private GameObject CreateMachineObject(string bName, int x, int y){
		GameObject tile = (GameObject)Instantiate(Resources.Load(bName), new Vector3(x,y,-0.1f), Quaternion.identity);
		Tile t = tile.GetComponent<Tile> ();
        t.SetTileName (bName);
		t.SetCost (costs[bName]);
		t.SetBuildable (false);
		return tile;
	}

	private GameObject CreateFloorObject(int x, int y){
		GameObject tile = (GameObject)Instantiate(Resources.Load("Metal"), new Vector2(x,y), Quaternion.identity);
		Tile t = tile.GetComponent<Tile> ();
        t.SetTileName ("Metal");
		t.SetCost (10);
		t.SetBuildable (true);
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
		selectionBox.GetComponent<SpriteRenderer> ().material.color = color;
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

    public GameObject[,] FactoryMap
    {
        get
        {
            return factoryMap;
        }
    }

    public double MoneyTotal {
		get {
			return moneyTotal;
		}
	}

    public double CurrentDemand
    {
        get
        {
            return currentDemand;
        }
    }

    public double PricePerKW
    {
        get
        {
            return pricePerKW;
        }
    }

    public void SetPowerDemand()
    {

    }

    public double PriceOfCoal
    {
        get
        {
            return priceOfCoal;
        }
    }
}
