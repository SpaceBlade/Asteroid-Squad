using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class BattleTurnSystem : MonoBehaviour {
	public enum TurnMode
	{
		BattleMode,
		TurnActions,
		EndTurn,
		EndBattle
	}

	// Prefabs
	public GameObject playerPrefab;
	public GameObject NPCPrefab;

    // Squads
    public Dictionary<string, GameObject[]> Squads = new Dictionary<string, GameObject[]>();
	private GameObject[] SquadAlpha;    // Player squad
    private GameObject[] SquadBeta; // NPC squad
    private GameObject[] SquadGamma;	// NPC squad 2

	// Turn
	public float TurnTimer;
	public TurnMode turnMode = TurnMode.BattleMode;

	// Turn camera
	public GameObject MainCamera;

	// Map
	public GameObject GameMap;
	// Private objects

	private int activePlayer = 0;
	private float remainingTurnTime = 0;

	private short TurnCount = 0;
	private string currentTurnString = "missing";


	// UI Objects updated by the Battle system
	public Text TimeRemainingText;
	public Slider TimeRemainingSlider;
	public Text NewRoundText;

	// Use this for initialization
	void Start () {
		NewRoundText.enabled = false;
		turnMode = TurnMode.BattleMode;
		TranslateTurnString (turnMode);
		remainingTurnTime = TurnTimer;

		TimeRemainingSlider.minValue = 0.0f;
		TimeRemainingSlider.maxValue = TurnTimer;
		TimeRemainingSlider.value = remainingTurnTime;
		/*
		GameObject[] spawningPoints = GameObject.FindGameObjectsWithTag ("SpawnPoints");
		if (spawningPoints != null && spawningPoints.Length > 0) {
			Debug.Log(string.Format("Found ({0:n}) spawning points", spawningPoints.Length));
			// Add NPC
			SquadBeta = new GameObject[spawningPoints.Length];
			for(int pt=0; pt < spawningPoints.Length; pt++){
				SquadBeta[pt] = (GameObject)Instantiate(NPCPrefab, spawningPoints[pt].transform.position, spawningPoints[pt].transform.rotation);
			}
		}
		*/


		//// Check for Team Alpha spawn points
		//var mapPoints = GameMap.GetComponent<MapProperties> ();
		//if(mapPoints.SpawnPointsAlpha != null && mapPoints.SpawnPointsAlpha.Length > 0){
		//	Debug.Log(string.Format("Team Alpha ({0:n}) spawning points", mapPoints.SpawnPointsAlpha.Length));
		//	// Add NPC
		//	SquadAlpha = new GameObject[mapPoints.SpawnPointsAlpha.Length];
		//	for(int pt=0; pt < mapPoints.SpawnPointsAlpha.Length; pt++){
		//		SquadAlpha[pt] = (GameObject)Instantiate(playerPrefab, mapPoints.SpawnPointsAlpha[pt].transform.position, mapPoints.SpawnPointsAlpha[pt].transform.rotation);
		//	}
		//}

		//// Check for Beta spawn points
		//if(mapPoints.SpawnPointsBeta != null && mapPoints.SpawnPointsBeta.Length > 0){
		//	Debug.Log(string.Format("Team Beta ({0:n}) spawning points", mapPoints.SpawnPointsBeta.Length));
		//	// Add NPC
		//	SquadBeta = new GameObject[mapPoints.SpawnPointsBeta.Length];
		//	for(int pt=0; pt < mapPoints.SpawnPointsAlpha.Length; pt++){
		//		SquadBeta[pt] = (GameObject)Instantiate(NPCPrefab, mapPoints.SpawnPointsBeta[pt].transform.position, mapPoints.SpawnPointsBeta[pt].transform.rotation);
		//	}
		//}



		ResetTurn ();
	}
	
	// Update is called once per frame
	void Update () {
        if (turnMode == TurnMode.EndBattle)
        {
            // End Battle
            StartCoroutine("EndBattleFade");
            return;
        }

        remainingTurnTime -= Time.deltaTime;
        if (turnMode == TurnMode.BattleMode) {
            
            // Update UI for BattleMode
            TimeRemainingSlider.value = remainingTurnTime;
		}

		// Switch player turn
		if (Input.GetKeyDown (KeyCode.Tab)) {
			SwitchActivePlayer();
		}

		// Check if turn ended
		if (remainingTurnTime < 0){
            if (turnMode == TurnMode.BattleMode)
			{
				// Set end-turn timer
				remainingTurnTime = 0.60f; // wait one second for physics collisions
				turnMode = TurnMode.TurnActions;
				TranslateTurnString(turnMode);

				// Disable actions
				// Reset timer of all objects
				GameObject[] squadmates = GameObject.FindGameObjectsWithTag("SquadMate");
				for(int i = 0; i < squadmates.Length; i++)
				{	
					var squadTurn = squadmates[i].GetComponent<SquadMemberTurn>();
					if(squadTurn != null){
						squadTurn.ToggleMovement(false);
						squadTurn.ToggleShooting(false);
					}
				}
			}
			else if(turnMode == TurnMode.TurnActions)
			{
				remainingTurnTime = 1.0f; // wait time for turn actions and animations

                // Reset Player
                foreach(var squad in Squads)
                {
                    for (int i = 0; i < squad.Value.Length; i++)
                    {
                        squad.Value[i].GetComponent<SquadMemberTurn>().ExecuteTurnActions();
                    }
                }


				//// Reset timer of all objects
				//if(SquadAlpha != null && SquadAlpha.Length > 0)
				//{
				//	for(int i = 0; i < SquadAlpha.Length; i++)
				//	{	
				//		SquadAlpha[i].GetComponent<SquadMemberTurn>().ExecuteTurnActions();
				//	}
				//}

				//if(SquadBeta != null && SquadBeta.Length > 0)
				//{
				//	for(int i = 0; i < SquadBeta.Length; i++)
				//	{	
				//		// SquadBeta[i].GetComponent<SquadMemberTurn>().ExecuteTurnActions();
				//	}
				//}

				//if(SquadGamma != null && SquadGamma.Length > 0){
				//	for(int i = 0; i < SquadGamma.Length; i++)
				//	{	
				//		// SquadBeta[i].GetComponent<SquadMemberTurn>().ExecuteTurnActions();
				//	}
				//}

				// Reset Turn
				turnMode = TurnMode.EndTurn;
				TranslateTurnString(turnMode);
			}
			else if(turnMode == TurnMode.EndTurn)
			{
				// End turn
				EndTurn();
				TurnCount++;
			}
		}

		TimeRemainingText.text = string.Format ("Turn {0:n0}: {1}\n{2:n2} seconds", TurnCount, currentTurnString, remainingTurnTime);
	}

	public void SwitchActivePlayer()
	{
		// Check if teams defined
		if (SquadAlpha != null && SquadAlpha.Length > 0) {
			// Reset active flag
			SquadMemberTurn currentSMT = SquadAlpha[activePlayer].GetComponent<SquadMemberTurn>();
            
            // Check if all players in team have been switched
            if (++activePlayer >= SquadAlpha.Length)
            {
                // Reset active player
                activePlayer = 0;
            }

            // Check if player is not dead
            SquadMemberTurn smt = SquadAlpha[activePlayer].GetComponent<SquadMemberTurn>();
            int maxTries = SquadAlpha.Length, currentTry = 0;
            while (!smt.playerStats.IsAlive && currentTry < maxTries)
            {
                // Check if all players in team have been switched
                if (++activePlayer >= SquadAlpha.Length)
                {
                    // Reset active player
                    activePlayer = 0;
                }
                
                smt = SquadAlpha[activePlayer].GetComponent<SquadMemberTurn>();
                currentTry++;
            }
            
            smt.IsActiveSquaddie = true;
            MainCamera.GetComponent<CameraController>().TrackedPlayer = SquadAlpha[activePlayer];

            // Reset last active
            currentSMT.IsActiveSquaddie = false;
		}
	}
    	
	public void EndTurn()
	{

        // check if alive
        //if (IsSquadAlive (SquadAlpha) && (IsSquadAlive (SquadBeta) || IsSquadAlive (SquadGamma))) {
        bool continueTurn = true;
        foreach(var squad in Squads)
        {
            continueTurn = continueTurn && IsSquadAlive(squad.Value);
        }

        if (continueTurn)
        { 
			NewRoundText.enabled = true;
			StartCoroutine("NewTurnFade");
			// Update Squads

			ResetTurn();
		} else {
			// End Battle
			EndBattle ();
		}
	}

	// Check if a squad has members alive 
	private bool IsSquadAlive(GameObject [] squad)
	{
		// Check if all players in a squad are dead
		if (squad != null && squad.Length > 0) {
			for(int ix = 0; ix < squad.Length; ix++){
                var squadStats = squad[ix].GetComponent<SquadStats>();
                if (squadStats != null && squadStats.Health > 0)
				{
					return true;
				}
			}
		}
		
		return false;
	}

	// Reset turn
	public void ResetTurn()
	{
		remainingTurnTime = TurnTimer;
		turnMode = TurnMode.BattleMode;
		TranslateTurnString (turnMode);

		// Reset timer of all objects
		GameObject[] squadmates = GameObject.FindGameObjectsWithTag("SquadMate");
		for(int i = 0; i < squadmates.Length; i++)
		{
			var squadTurn = squadmates[i].GetComponent<SquadMemberTurn>();
			if(squadTurn != null && squadTurn.playerStats.IsAlive)
			{
				squadTurn.ResetTurn();
			}
		}

        // If any members have been added to squad then add to dictionary
        if (SquadAlpha == null)
        {
            if (Squads.ContainsKey("SquadAlpha") && Squads["SquadAlpha"].Length > 0)
            {
                SquadAlpha = Squads["SquadAlpha"];
                // start first player to move
                SquadAlpha[activePlayer].GetComponent<SquadMemberTurn>().IsActiveSquaddie = true;
                MainCamera.GetComponent<CameraController>().TrackedPlayer = SquadAlpha[activePlayer];
            }
        }
    }

	// Battle is over
	public void EndBattle()
	{
		turnMode = TurnMode.EndBattle;	// End battle
		TranslateTurnString (turnMode);
	}

	public TurnMode GetActiveTurnMode()
	{
		return turnMode;
	}

	private void TranslateTurnString(TurnMode tm)
	{
		switch (tm) {
		case TurnMode.BattleMode:
			currentTurnString = "Battle";
			break;
		case TurnMode.EndBattle:
			currentTurnString = "End Battle";
			break;
		case TurnMode.EndTurn:
			currentTurnString = "End turn";
			break;
		case TurnMode.TurnActions:
			currentTurnString = "Turn Actions";
			break;
		default:
			currentTurnString = "Battle";
			break;
		}
	}

	// Displayed at the beginning of each turn
    private IEnumerator NewTurnFade()
	{
		NewRoundText.enabled = true;
		for(float sc = 0.0f; sc < 4.0f; sc+= 0.1f)
		{
			NewRoundText.transform.localScale  = Vector3.one * sc;
			// yield return new WaitForSeconds(0.1f);
			yield return null;
		}

		NewRoundText.enabled = false;
	} // NewTurnFade()

    private IEnumerator EndBattleFade()
    {
        NewRoundText.enabled = true;

        NewRoundText.text = "Your squad lost!";
        if (IsSquadAlive(SquadAlpha))
        {
            NewRoundText.text = "Winner!";
        }

        for (float sc = 0.0f; sc < 14.0f; sc += 0.1f)
        {
            NewRoundText.transform.localScale = Vector3.one * sc;
            // yield return new WaitForSeconds(0.1f);
            yield return null;
        }

        NewRoundText.enabled = false;
    } // EndBattleFade()
}
