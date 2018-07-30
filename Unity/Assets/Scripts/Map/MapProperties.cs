using UnityEngine;


public class MapProperties : MonoBehaviour {
	// collection of spawn points in map
    public GameObject AlphaGenerator;
    public GameObject BetaGenerator;
    public GameObject GammaGenerator;
    public BattleTurnSystem BattleTurnSystem;
    public Texture2D MapLevel;
    public GameObject[] MapTiles;

    private GameObject[] generatedMap;

    public static readonly Vector3 TileSize = new Vector3(10.0f, 10.0f, 10.0f);

    // Use this for initialization
    void Start () {
	    if(MapLevel != null && ! string.IsNullOrEmpty(MapLevel.name))
        {
            generatedMap = new GameObject[MapLevel.width * MapLevel.height];
            for (int coordX=0; coordX < MapLevel.width; coordX++)
            {
                for(int coordY = 0; coordY < MapLevel.height; coordY++)
                {
                    generatedMap[coordX * MapLevel.width + coordY] = GameObject.Instantiate(MapTiles[0]);
                    generatedMap[coordX * MapLevel.width + coordY].transform.position += new Vector3(10 * coordX, 0, 10 * coordY);
                    //var tile = GameObject.Instantiate(MapTiles[0]);
                    // tile.transform.position += new Vector3(10 * coordX, 0, 10 * coordY);
                    
                }
            }
        }



        SetupNavigation();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetupNavigation()
    {
        NavMesh.CalculateTriangulation();
    }
}
