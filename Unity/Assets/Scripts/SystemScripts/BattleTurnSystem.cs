using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleTurnSystem : MonoBehaviour {
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

	private PlayerInputScript pis;
	private int activePlayer = 0;
	private float remainingTurnTime = 0;


	// UI Objects updated by the Battle system
	public Text TimeRemainingText;

	// Use this for initialization
	void Start () {
		remainingTurnTime = TurnTimer;
		pis = GetComponent<PlayerInputScript> ();

		// If any members have been added to squad then add to dictionary
		if (SquadAlpha != null && SquadAlpha.Length > 0) {
			// start first player to move
			pis.targetPlayer = SquadAlpha[activePlayer];
			SquadAlpha[activePlayer].GetComponent<SquadMemberTurn>().IsActiveSquaddie = true;

			// Create copy
			/* GameObject dupe = (GameObject)GameObject.Instantiate(squadAlpha[activePlayer]);
			dupe.transform.position += new Vector3(20.0f, 0.0f, 50.0f);
			dupe.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
			*/
			MainCamera.GetComponent<CameraController>().TrackedPlayer = pis.targetPlayer;
		}

		ResetTurn ();
	}
	
	// Update is called once per frame
	void Update () {
		remainingTurnTime -= Time.deltaTime;

		// Check if turn ended
		if (remainingTurnTime < 0) {
			remainingTurnTime = TurnTimer;

			// End turn
			EndTurn();

		}
		TimeRemainingText.text = string.Format ("{0:n2} seconds", remainingTurnTime);
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
		// Execute turn actions


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
		// Reset timer of all objects
		GameObject[] squadmates = GameObject.FindGameObjectsWithTag("SquadMate");
		for(int i = 0; i < squadmates.Length; i++)
		{
			var squadTurn = squadmates[i].GetComponent<SquadMemberTurn>();
			if(squadTurn.IsAlive)
			{
				squadTurn.ResetTurnTime();
			}
		}
	}

	// Battle is over
	public void EndBattle()
	{
	}
}
