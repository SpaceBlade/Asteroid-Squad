using UnityEngine;
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
		// Check for Team Alpha spawn points
		var mapPoints = GameMap.GetComponent<MapProperties> ();
		if(mapPoints.SpawnPointsAlpha != null && mapPoints.SpawnPointsAlpha.Length > 0){
			Debug.Log(string.Format("Team Alpha ({0:n}) spawning points", mapPoints.SpawnPointsAlpha.Length));
			// Add NPC
			SquadAlpha = new GameObject[mapPoints.SpawnPointsAlpha.Length];
			for(int pt=0; pt < mapPoints.SpawnPointsAlpha.Length; pt++){
				SquadAlpha[pt] = (GameObject)Instantiate(playerPrefab, mapPoints.SpawnPointsAlpha[pt].transform.position, mapPoints.SpawnPointsAlpha[pt].transform.rotation);
			}
		}

		// Check for Beta spawn points
		if(mapPoints.SpawnPointsAlpha != null && mapPoints.SpawnPointsBeta.Length > 0){
			Debug.Log(string.Format("Team Beta ({0:n}) spawning points", mapPoints.SpawnPointsBeta.Length));
			// Add NPC
			SquadBeta = new GameObject[mapPoints.SpawnPointsBeta.Length];
			for(int pt=0; pt < mapPoints.SpawnPointsAlpha.Length; pt++){
				SquadBeta[pt] = (GameObject)Instantiate(NPCPrefab, mapPoints.SpawnPointsBeta[pt].transform.position, mapPoints.SpawnPointsBeta[pt].transform.rotation);
			}
		}


		// If any members have been added to squad then add to dictionary
		if (SquadAlpha != null && SquadAlpha.Length > 0) {
			// start first player to move
			SquadAlpha[activePlayer].GetComponent<SquadMemberTurn>().IsActiveSquaddie = true;
			MainCamera.GetComponent<CameraController>().TrackedPlayer = SquadAlpha[activePlayer];
		}

		ResetTurn ();
	}
	
	// Update is called once per frame
	void Update () {
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
			if(turnMode == TurnMode.BattleMode)
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
		if (SquadAlpha.Length > 0) {
			// Reset active flag
			SquadAlpha[activePlayer].GetComponent<SquadMemberTurn>().IsActiveSquaddie = false;

			 // Check if all players in team have been switched
			if(++activePlayer >= SquadAlpha.Length)
			{
				// Reset active player
				activePlayer = 0;
			}

			MainCamera.GetComponent<CameraController>().TrackedPlayer = SquadAlpha[activePlayer];
			SquadAlpha[activePlayer].GetComponent<SquadMemberTurn>().IsActiveSquaddie = true;
		}
	}

	public void EndPlayerturn()
	{
	}

	public void EndTurn()
	{

		// check if alive
		if (IsSquadAlive (SquadAlpha) && (IsSquadAlive (SquadBeta) || IsSquadAlive (SquadGamma))) {
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
			currentTurnString = "Turn Actions";
			break;
		case TurnMode.EndTurn:
			currentTurnString = "Turn Actions";
			break;
		case TurnMode.TurnActions:
			currentTurnString = "Turn Actions";
			break;
		default:
			currentTurnString = "Battle";
			break;
		}
	}

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
	}
}
