using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour {
	protected static GameObject manager;
    protected static SessionManager sm;
    Texture2D texture;
	string tileName;
	double cost;
    protected double maintenanceCost = 0;
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

	public static void SetManager(GameObject m){
		manager = m;
        sm = manager.GetComponent<SessionManager>();
    }

	public double KwProduced {
		get {
			return kwProduced;
		}
	}

    public double MaintenanceCost
    {
        get
        {
            return maintenanceCost;
        }
    }

    public abstract double PerformDailyMaintenance();

    public GameObject[] GetNeighbors() {
        GameObject[,] map = sm.FactoryMap;
        GameObject[] neighbors = new GameObject[4];
        int x = (int)this.transform.position.x;
        int y = (int)this.transform.position.y;
        int nCount = 0;
        // Right neighbor
        if(x < map.GetLength(0) - 1 && map[x + 1, y] != null)
        {
            neighbors[nCount] = map[x + 1, y];
            nCount++;
        }
        // Left Neighbor
        if (x > 0 && map[x - 1, y] != null)
        {
            neighbors[nCount] = map[x - 1, y];
            nCount++;
        }
        // Top Neighbor
        if (y < map.GetLength(1) - 1 && map[x, y + 1] != null)
        {
            neighbors[nCount] = map[x, y + 1];
            nCount++;
        }
        // Bottom Neighbor
        if (y > 0 && map[x, y - 1] != null)
        {
            neighbors[nCount] = map[x, y - 1];
            nCount++;
        }
        GameObject[] nn = new GameObject[nCount];
        for(int i = 0; i < nCount; i++)
        {
            nn[i] = neighbors[i];
        }
        return nn;
    }

	void OnMouseOver(){
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
