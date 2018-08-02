using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boiler : Tile {
	double steamProduced = 8.142;
    double steamUsed = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    public double GetAvailableSteam()
    {
        //Debug.Log("Boiler at " + this.transform.position.x + ", " + this.transform.position.y + " has " + (steamProduced - steamUsed) + " available steam.");
        return steamProduced - steamUsed;
    }

    public void UseSteam(double steam)
    {
        steamUsed = Math.Min(steamUsed+steam, steamProduced);
        //Debug.Log("Boiler at " + this.transform.position.x + ", " + this.transform.position.y + " using " + steamUsed + " steam");
    }
    override
    public double PerformDailyMaintenance()
    {
        return 24*sm.PriceOfCoal + 1.50;
    }
}
