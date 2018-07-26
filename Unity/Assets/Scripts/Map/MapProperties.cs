using UnityEngine;
using System.Collections;

public class MapProperties : MonoBehaviour {
	// collection of spawn points in map
    public GameObject AlphaGenerator;
    public GameObject BetaGenerator;
    public GameObject GammaGenerator;
    public BattleTurnSystem BattleTurnSystem;
    public Texture2D MapLevel;
    public GameObject[] MapTiles;

	// Use this for initialization
	void Start () {
	    if(MapLevel != null && ! string.IsNullOrEmpty(MapLevel.name))
        {
            for(int coordX=0; coordX < MapLevel.width; coordX++)
            {
                for(int coordY = 0; coordY < MapLevel.height; coordY++)
                {
                    var tile = GameObject.Instantiate(MapTiles[0]);
                    tile.transform.position += new Vector3(10 * coordX, 0, 10 * coordY);
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
