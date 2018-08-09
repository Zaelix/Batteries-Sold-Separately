﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHandler : MonoBehaviour {
	public GameObject powerDisplay;
	public GameObject moneyDisplay;
    public GameObject dayNightDisplay;
    public GameObject maintCostDisplay;
    public GameObject dailyProfitDisplay;
    public GameObject timeDisplay;

    public GameObject priceDisplay;
    public GameObject demandDisplay;
    public GameObject supplyDisplay;

    // Build Menu stuff
    public GameObject boilerBuildMenu;
    public GameObject turbineBuildMenu;
	SessionManager sm;

    private double produced = 0;
    private float time;
    private int hour = 0;
    string ampm = "am";
    double dailyMaintenanceCost = 0;
    double dailyIncome = 0;

	// Use this for initialization
	void Start () {
		sm = this.gameObject.GetComponent<SessionManager>();
	}
	
	// Update is called once per frame
	void Update () {
		powerDisplay.GetComponent<Text> ().text = sm.PowerTotal.ToString("0.000") + " KW";
		moneyDisplay.GetComponent<Text> ().text = sm.MoneyTotal.ToString("F");
        timeDisplay.GetComponent<Text>().text = ((int)(time % 12)+1).ToString("00") + ampm;
        priceDisplay.GetComponent<Text>().text = "Price: " + sm.PricePerKW.ToString("0.0000");
        demandDisplay.GetComponent<Text>().text = "Demand: " + sm.CurrentDemand.ToString("F") + " KW";
        maintCostDisplay.GetComponent<Text>().text = dailyMaintenanceCost.ToString("0.00");
        dailyProfitDisplay.GetComponent<Text>().text = dailyIncome.ToString("0.00");
        supplyDisplay.GetComponent<Text>().text = "Supply: " + sm.MarketPowerSupply + " KW";
    }

    void FixedUpdate()
    {
        dayNightDisplay.GetComponent<Slider>().value += 0.01f;
        time = dayNightDisplay.GetComponent<Slider>().value;
        dayNightDisplay.GetComponentInChildren<Image>().color = (Color.blue * (1- (Distance(time, 12)/12)))+(Color.black * (Distance(time, 12) / 12));
        //Debug.Log("Hour is: " + hour + ", Time is: " + time);
        if ((int)time > hour)
        {
            Debug.Log("Hour " + hour + ": " + sm.CountPowerProduced() + " kw/h produced this hour.");
        }
        if (time >= 24)
        {
            ampm = "am";
            dayNightDisplay.GetComponent<Slider>().value = 0;
            dailyMaintenanceCost = sm.PerformMaintenance();
            dailyIncome = sm.CalculateDailyIncome();
        }
        else if(time >= 12)
        {
            ampm = "pm";
        }
        sm.CountPower();
        hour = (int)time;
    }

    float Distance(float a, float b)
    {
        return Mathf.Abs(a - b);
    }

    public float GetTime()
    {
        return this.time;
    }

    public void DisplayBuildMenu(string menu)
    {
        bool menuState = false;
        switch (menu)
        {
            case "Boilers":
                menuState = boilerBuildMenu.activeInHierarchy;
                break;
            case "Turbines":
                menuState = turbineBuildMenu.activeInHierarchy;
                break;
            default:
                break;
        }
        CloseAllMenus();
        switch (menu)
        {
            case "Boilers":
                boilerBuildMenu.SetActive(!menuState);
                break;
            case "Turbines":
                turbineBuildMenu.SetActive(!menuState);
                break;
            default:
                break;
        }
    }

    private void CloseAllMenus()
    {
        boilerBuildMenu.SetActive(false);
        turbineBuildMenu.SetActive(false);
    }

    public void SelectBuildingFromMenu(string type)
    {
        sm.SetBuilding(type);
    }
}
