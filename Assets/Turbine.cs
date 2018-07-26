using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbine : Tile {
	public double steam = 0;
	public double maxSteam = 20;
    double kwPerSteamUnit = 1;

    // Use this for initialization
    void Start () {
		kwProduced = 0;
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
            }
        }
        steam += steamUnits;
        kwProduced = steam * kwPerSteamUnit;
    }
}
