using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleTurnSystem : MonoBehaviour {
	// Squads
	public GameObject[] squadAlpha;
	public GameObject[] squadBeta;

	// Turn camera
	public GameObject MainCamera;

	// Private objects
	private Dictionary<string, GameObject[]> battleSquads;
	private PlayerInputScript pis;
	private int activePlayer = 0;
	private float remainingTurnTime = 30;

	// UI Objects updated by the Battle system
	public Text TimeRemainingText;

	// Use this for initialization
	void Start () {
		pis = GetComponent<PlayerInputScript> ();
		battleSquads = new Dictionary<string, GameObject[]> ();
		// If any members have been added to squad then add to dictionary
		if (squadAlpha != null && squadAlpha.Length > 0) {
			battleSquads.Add("squadAlpha", squadAlpha);

			// start first player to move
			pis.targetPlayer = squadAlpha[activePlayer];
			// Create copy
			GameObject dupe = (GameObject)GameObject.Instantiate(squadAlpha[activePlayer]);
			dupe.transform.position += new Vector3(20.0f, 0.0f, 50.0f);
			dupe.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

			MainCamera.GetComponent<CameraController>().TrackedPlayer = pis.targetPlayer;
		}
	}
	
	// Update is called once per frame
	void Update () {
		float remaining = Time.deltaTime;
		remainingTurnTime -= Time.deltaTime;
		if (remainingTurnTime < 0) {
			remainingTurnTime = 30.0f;
		}
		TimeRemainingText.text = string.Format ("{0:n2} seconds", remainingTurnTime);
	}

	public void EndPlayerturn()
	{
		// Check if teams defined
		if (battleSquads.Count > 0) {
			// Check if all players in team have had a turn
			if(++activePlayer >= battleSquads["squadAlpha"].Length)
			{
				// Reset active player
				activePlayer = 0;
			}

			pis.targetPlayer = squadAlpha[activePlayer];
			MainCamera.GetComponent<CameraController>().TrackedPlayer = pis.targetPlayer;
		}
	}
}
