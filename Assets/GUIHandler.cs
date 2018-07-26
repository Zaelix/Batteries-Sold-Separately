using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHandler : MonoBehaviour {
	public GameObject powerDisplay;
	public GameObject moneyDisplay;
    public GameObject dayNightDisplay;
	SessionManager sm;
	// Use this for initialization
	void Start () {
		sm = this.gameObject.GetComponent<SessionManager>();
	}
	
	// Update is called once per frame
	void Update () {
		powerDisplay.GetComponent<Text> ().text = sm.PowerTotal + " KW/H";
		moneyDisplay.GetComponent<Text> ().text = sm.MoneyTotal.ToString("F");
	}

    void FixedUpdate()
    {
        dayNightDisplay.GetComponent<Slider>().value += 0.05f;
        float time = dayNightDisplay.GetComponent<Slider>().value;
        dayNightDisplay.GetComponentInChildren<Image>().color = (Color.white * (1- (Distance(time, 12)/12)))+(Color.black * (Distance(time, 12) / 12));
        if (time >= 24)
        {
            dayNightDisplay.GetComponent<Slider>().value = 0;
        }
    }

    float Distance(float a, float b)
    {
        return Mathf.Abs(a - b);
    }
}
