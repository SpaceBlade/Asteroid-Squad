using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleTurnSystem : MonoBehaviour {
	// Squads
	public GameObject[] battleSquads;

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
		if (battleSquads != null && battleSquads.Length > 0) {
			// start first player to move
			pis.targetPlayer = battleSquads[0].GetComponent<SquadTeam>().SquadMembers[activePlayer];

			// Create copy
			/* GameObject dupe = (GameObject)GameObject.Instantiate(squadAlpha[activePlayer]);
			dupe.transform.position += new Vector3(20.0f, 0.0f, 50.0f);
			dupe.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
			*/
			MainCamera.GetComponent<CameraController>().TrackedPlayer = pis.targetPlayer;
		}
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
		if (battleSquads.Length > 0) {
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

		// Check if all players in a squad are dead
		bool squadAlive = false;
		// for (int sqIx = 0; sqIx < battleSquads.Length; sqIx++) {
		for (int sqIx = 0; sqIx < 1; sqIx++) {
			GameObject[] squad = battleSquads[sqIx].GetComponent<SquadTeam>().SquadMembers;
			for(int squaddie = 0; squaddie < squad.Length; squaddie++)
			{
				// check if alive
				if(squad[squaddie].GetComponent<SquadStats>().Health > 0)
				{
					// Check if other squad is alive
					if(squadAlive)
					{
						// Reset Turn at the end
						ResetTurn ();
					}

					squadAlive = true;
					break;
				}
			}
		}

		// End Battle
		EndBattle ();
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
				squadTurn.ToggleMovement(true);
				squadTurn.ResetTurnTime();
			}
		}
	}

	// Battle is over
	public void EndBattle()
	{
	}
}
