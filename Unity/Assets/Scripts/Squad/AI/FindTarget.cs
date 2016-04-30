using UnityEngine;
using System.Collections;

public class FindTarget : MonoBehaviour {
	public SquadMemberTurn squadTurn;
	public BattleTurnSystem battleSys;
    public GameObject currentTarget = null;

	// Use this for initialization
	void Start () {
		squadTurn = Component.FindObjectOfType<SquadMemberTurn> ();
        if (squadTurn != null)
        {
            battleSys = squadTurn.battleTurnManager;
        }
	}
	
	// Update is called once per frame
	void Update () {
		// Find New Target
        if (currentTarget == null)
        {
            if (battleSys.SquadAlpha.Length > 0) // Check for players
            {
                currentTarget = battleSys.SquadAlpha[0];
            }
        }
	}

	private GameObject FindTargetSquad()
	{
		return null;
	}
}
