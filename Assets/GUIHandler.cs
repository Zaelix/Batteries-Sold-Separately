using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHandler : MonoBehaviour {
	public GameObject powerDisplay;
	public GameObject moneyDisplay;
    public GameObject dayNightDisplay;
    public GameObject timeDisplay;
    public GameObject priceDisplay;
    public GameObject demandDisplay;
	SessionManager sm;

    private float time;
    string ampm = "am";

	// Use this for initialization
	void Start () {
		sm = this.gameObject.GetComponent<SessionManager>();
	}
	
	// Update is called once per frame
	void Update () {
		powerDisplay.GetComponent<Text> ().text = sm.PowerTotal + " KW/H";
		moneyDisplay.GetComponent<Text> ().text = sm.MoneyTotal.ToString("F");
        timeDisplay.GetComponent<Text>().text = ((int)(time % 12)+1).ToString("00") + ampm;
        priceDisplay.GetComponent<Text>().text = "Price: " + sm.PricePerKW.ToString("0.00");
        demandDisplay.GetComponent<Text>().text = "Demand:" + sm.CurrentDemand.ToString("F");
    }

    void FixedUpdate()
    {
        dayNightDisplay.GetComponent<Slider>().value += 0.01f;
        time = dayNightDisplay.GetComponent<Slider>().value;
        dayNightDisplay.GetComponentInChildren<Image>().color = (Color.blue * (1- (Distance(time, 12)/12)))+(Color.black * (Distance(time, 12) / 12));
        if (time >= 24)
        {
            ampm = "am";
            dayNightDisplay.GetComponent<Slider>().value = 0;
        }
        else if(time >= 12)
        {
            ampm = "pm";
        }
    }

    float Distance(float a, float b)
    {
        return Mathf.Abs(a - b);
    }

    public float GetTime()
    {
        return this.time;
    }
}
