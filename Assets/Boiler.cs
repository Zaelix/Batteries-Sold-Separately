using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boiler : Tile {
	double steamProduced = 5;
    double steamUsed = 0;
	// Use this for initialization
	void Start () {
		steamProduced = 5;
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
        steamUsed = Math.Min(steamUsed+steam, 5);
        //Debug.Log("Boiler at " + this.transform.position.x + ", " + this.transform.position.y + " using " + steamUsed + " steam");
    }
}
