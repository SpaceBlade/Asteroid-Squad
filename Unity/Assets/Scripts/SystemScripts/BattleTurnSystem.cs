﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
	public GameObject[] SquadAlpha;	// Player squad
	public GameObject[] SquadBeta;	// NPC squad
	public GameObject[] SquadGamma;	// NPC squad 2

	// Turn
	public float TurnTimer;

	// Turn camera
	public GameObject MainCamera;

	// Map
	public GameObject GameMap;
	// Private objects

	private int activePlayer = 0;
	private float remainingTurnTime = 0;
	private TurnMode turnMode = TurnMode.BattleMode;
	private short TurnCount = 0;


	// UI Objects updated by the Battle system
	public Text TimeRemainingText;

	// Use this for initialization
	void Start () {
		turnMode = TurnMode.BattleMode;
		remainingTurnTime = TurnTimer;
		GameObject[] spawningPoints = GameObject.FindGameObjectsWithTag ("SpawnPoints");
		if (spawningPoints != null && spawningPoints.Length > 0) {
			Debug.Log(string.Format("Found ({0:n}) spawning points", spawningPoints.Length));
			// Add NPC
			SquadBeta = new GameObject[spawningPoints.Length];
			for(int pt=0; pt < spawningPoints.Length; pt++){
				SquadBeta[pt] = (GameObject)Instantiate(NPCPrefab, spawningPoints[pt].transform.position, spawningPoints[pt].transform.rotation);
			}
		}





		// If any members have been added to squad then add to dictionary
		if (SquadAlpha != null && SquadAlpha.Length > 0) {
			// start first player to move
			SquadAlpha[activePlayer].GetComponent<SquadMemberTurn>().IsActiveSquaddie = true;

			// Create copy
			/* GameObject dupe = (GameObject)GameObject.Instantiate(squadAlpha[activePlayer]);
			dupe.transform.position += new Vector3(20.0f, 0.0f, 50.0f);
			dupe.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
			*/
			MainCamera.GetComponent<CameraController>().TrackedPlayer = SquadAlpha[activePlayer];
		}

		ResetTurn ();
	}
	
	// Update is called once per frame
	void Update () {
		remainingTurnTime -= Time.deltaTime;

		// Switch player turn
		if (Input.GetKeyDown (KeyCode.Tab)) {
			SwitchActivePlayer();
		}

		// Check if turn ended
		if (remainingTurnTime < 0){
			if(turnMode == TurnMode.BattleMode)
			{
				// Set end-turn timer
				remainingTurnTime = 0.60f; // wait one second for physics collisions
				turnMode = TurnMode.TurnActions;

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

				// Reset timer of all objects
				if(SquadAlpha != null && SquadAlpha.Length > 0)
				{
					for(int i = 0; i < SquadAlpha.Length; i++)
					{	
						SquadAlpha[i].GetComponent<SquadMemberTurn>().ExecuteTurnActions();
					}
				}

				if(SquadBeta != null && SquadBeta.Length > 0)
				{
					for(int i = 0; i < SquadBeta.Length; i++)
					{	
						// SquadBeta[i].GetComponent<SquadMemberTurn>().ExecuteTurnActions();
					}
				}

				if(SquadGamma != null && SquadGamma.Length > 0){
					for(int i = 0; i < SquadGamma.Length; i++)
					{	
						// SquadBeta[i].GetComponent<SquadMemberTurn>().ExecuteTurnActions();
					}
				}

				// Reset Turn
				turnMode = TurnMode.EndTurn;
			}
			else if(turnMode == TurnMode.EndTurn)
			{
				// End turn
				EndTurn();
				TurnCount++;
			}
		}

		TimeRemainingText.text = string.Format ("TurnMode:{0:n0}\n{1:n2} seconds", (int)turnMode, remainingTurnTime);
	}

	public void SwitchActivePlayer()
	{
		// Check if teams defined
		if (SquadAlpha.Length > 0) {
			/*
			 // Check if all players in team have had a turn
			if(++activePlayer >= battleSquads["squadAlpha"].Length)
			{
				// Reset active player
				activePlayer = 0;
			}

			pis.targetPlayer = squadAlpha[activePlayer];
			MainCamera.GetComponent<CameraController>().TrackedPlayer = pis.targetPlayer;
			*/
		}
	}

	public void EndPlayerturn()
	{
	}

	public void EndTurn()
	{
		// check if alive
		if (IsSquadAlive (SquadAlpha) && (IsSquadAlive (SquadBeta) || IsSquadAlive (SquadGamma))) {

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
		if (squad != null || squad.Length > 0) {
			for(int ix = 0; ix < squad.Length; ix++){
				if(squad[ix].GetComponent<SquadStats>().Health > 0)
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

		// Reset timer of all objects
		GameObject[] squadmates = GameObject.FindGameObjectsWithTag("SquadMate");
		for(int i = 0; i < squadmates.Length; i++)
		{
			var squadTurn = squadmates[i].GetComponent<SquadMemberTurn>();
			if(squadTurn != null && squadTurn.IsAlive)
			{
				squadTurn.ResetTurn();
			}
		}
	}

	// Battle is over
	public void EndBattle()
	{
	}

	public TurnMode GetActiveTurnMode()
	{
		return turnMode;
	}
}
