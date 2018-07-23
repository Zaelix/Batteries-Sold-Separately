using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	GameObject manager;
	Texture2D texture;
	string tileName;
	double cost;
	protected double kwProduced = 0;

	bool canBeBuiltOn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){

	}

	public void SetTexture(Texture2D tex){
		this.texture = tex;
	}

	public string TileName {
		get {
			return tileName;
		}
	}

	public void SetTileName(string name){
		this.tileName = name;
	}

	public void SetCost(double cost){
		this.cost = cost;
	}

	public void SetBuildable(bool canBeBuiltOn){
		this.canBeBuiltOn = canBeBuiltOn;
	}

	public bool IsBuildable(){
		return canBeBuiltOn;
	}

	public void SetManager(GameObject m){
		this.manager = m;
	}

	public double KwProduced {
		get {
			return kwProduced;
		}
	}

	void OnMouseOver(){
		SessionManager sm = manager.GetComponent<SessionManager> ();
		sm.SetSelection (this.gameObject);
		if (sm.GetBuildStatus ()) {
			//this.GetComponent<Renderer> ().material.shader = Shader.Find ("Particles/VertexLit Blended");
			if (this.canBeBuiltOn) {
				sm.SetSelectionColor (new Color (0, 1, 0));
			} else {
				sm.SetSelectionColor (new Color (1, 0, 0));
			}
		} else {
			sm.SetSelectionColor(new Color(0.1f,0.1f,1f));
		}
	}

	void OnMouseExit(){
		//this.GetComponent<Renderer>().material.shader = Shader.Find ("Sprites/Default");
	}
}
