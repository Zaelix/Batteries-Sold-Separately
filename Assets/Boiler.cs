﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boiler : Tile {
	double steamProduced = 8.142;
    double steamUsed = 0;
    double coalUsed;
    int outletPressure = 0;
    int outletTemperature = 0;

	// Use this for initialization
	void Start () {
        switch (TileName)
        {
            case "Hobbyist Boiler":
                steamProduced = 8.142;
                maintenanceCost = 1.50;
                outletPressure = 70;
                outletTemperature = 225;
                coalUsed = 1;
                break;
            case "Industrial Boiler":
                steamProduced = 80;
                maintenanceCost = 5.50;
                outletPressure = 150;
                outletTemperature = 400;
                coalUsed = 8;
                break;
            default:
                break;
        }
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
        return 24*(sm.PriceOfCoal*coalUsed) + maintenanceCost;
    }
}
