using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbine : Tile {
	public double steam = 0;
	public double maxSteam = 35;
    double kwPerSteamUnit = 1;

    double isentropicEfficiency = 0.80;
    double generatorEfficiency = 0.95;
    

    // Use this for initialization
    void Start () {
		kwProduced = 0;
        maintenanceCost = 2.50;
        switch (TileName)
        {
            case "Hobbyist Turbine":
                isentropicEfficiency = 0.70;
                generatorEfficiency = 0.92;
                maxSteam = 35;
                break;
            case "Industrial Turbine":
                isentropicEfficiency = 0.80;
                generatorEfficiency = 0.95;
                maxSteam = 320;
                break;
            default:
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        double steamUnits = 0;
        GameObject[] neighbors = GetNeighbors();
        foreach (GameObject obj in neighbors)
        {
            if(obj.GetComponent<Tile>().GetType() == typeof(Boiler))
            {
                Boiler b = obj.GetComponent<Boiler>();
                double avail = b.GetAvailableSteam();
                if (avail <= maxSteam - steam && avail > 0)
                {
                    steamUnits += avail;
                    b.UseSteam(avail);
                }
                else if(avail > 0)
                {
                    steamUnits += maxSteam - steam;
                    b.UseSteam(maxSteam - steam);
                }
               // Debug.Log("Steam: " + steam + " / " + maxSteam);
            }
        }
        steam += steamUnits;
        kwProduced = steam * kwPerSteamUnit * generatorEfficiency;
    }
    
    public override double PerformDailyMaintenance()
    {
        return maintenanceCost;
    }
    
    public override void Configure()
    {

    }
}
