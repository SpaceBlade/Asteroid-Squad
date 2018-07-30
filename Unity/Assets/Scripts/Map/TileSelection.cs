using UnityEngine;
using System.Collections;
using System;

public class TileSelection : MonoBehaviour {

    private MapProperties mapProperties;
    private BattleTurnSystem battleTurnSystem;
    public Camera camera;
    public GameObject tileHighlighter;

	// Use this for initialization
	void Start () {
        mapProperties = GetComponentInParent<MapProperties>();
        battleTurnSystem = mapProperties.BattleTurnSystem;
        tileHighlighter.GetComponent<MeshRenderer>();

        tileHighlighter.GetComponent<Material>().color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) || Input.touchCount == 1)
        {
            UnitSelection();
        }

        if (Input.GetMouseButtonDown(1) || Input.touchCount == 2)
        {
            UnitMove();
        }
	}

    private void UnitSelection()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayInfo;

        if(Physics.Raycast(ray, out rayInfo))
        {
            if(string.Compare(rayInfo.rigidbody.gameObject.tag, "SquadMate", System.StringComparison.OrdinalIgnoreCase) == 0)
            {
                var squaddie = rayInfo.rigidbody.gameObject.GetComponent<SquadMemberTurn>();
                // squaddie.IsActiveSquaddie = true;
                battleTurnSystem.SwitchActivePlayer(rayInfo.rigidbody.gameObject);
                tileHighlighter.transform.position = rayInfo.rigidbody.gameObject.transform.position + Vector3.up * 10.0f;

            }
        }
    }

    private void UnitMove()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayInfo;

        if (Physics.Raycast(ray, out rayInfo))
        {
            var activeplayer = battleTurnSystem.GetActivePlayer();
            var playerNavMesh = activeplayer.GetComponent<NavMeshAgent>();
            // playerNavMesh.SetDestination(rayInfo.transform.position);
            
        }
    }
}
